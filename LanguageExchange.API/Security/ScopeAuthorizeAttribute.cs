using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.IdentityModel;

namespace LanguageExchange.Security
{
    public class ScopeAuthorizeAttribute : AuthorizeAttribute
    {
        private string[] _scope;

        public ScopeAuthorizeAttribute() { }
        public ScopeAuthorizeAttribute(params string[] scope)
        {
            _scope = scope;
        }


        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.RequestContext.Principal != null &&
                actionContext.ControllerContext.RequestContext.Principal.Identity != null &&
                actionContext.ControllerContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Forbidden");
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            ClaimsPrincipal principal = (ClaimsPrincipal)actionContext.RequestContext.Principal;
            if(principal == null)
            {
                return false;
            }

            var accessClaim = principal.Claims.FirstOrDefault(c => c.Type == "scope");
            if(accessClaim == null)
            {
                return false;
            }

            if (!_scope.Contains(accessClaim.Value))
            {
                return false;
            }

            return true;
        }
    }
}
