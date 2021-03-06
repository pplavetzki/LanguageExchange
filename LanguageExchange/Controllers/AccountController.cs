﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using LanguageExchange.Models;
using LanguageExchange.Providers;
using LanguageExchange.Results;
using LanguageExchange.Services;
using LanguageExchange.Models.Dtos;
using LanguageExchange.Repository;
using Microsoft.Azure.Documents.Client;
using System.Configuration;
using StackExchange.Redis;
using LanguageExchange.Security;
using LanguageExchange.Interfaces;

namespace LanguageExchange.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController, IDisposable
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IRedisRepository _redis;

        public AccountController(IUserRepository userRepository, IRedisRepository redis)
        {
            _redis = redis;
            _userRepository = userRepository;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

        //    return Ok();
        //}

        //// POST api/Account/AddExternalLogin
        //[Route("AddExternalLogin")]
        //public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

        //    AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

        //    if (ticket == null || ticket.Identity == null || (ticket.Properties != null
        //        && ticket.Properties.ExpiresUtc.HasValue
        //        && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
        //    {
        //        return BadRequest("External login failure.");
        //    }

        //    ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

        //    if (externalData == null)
        //    {
        //        return BadRequest("The external login is already associated with an account.");
        //    }

        //    IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
        //        new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

        //    if (!result.Succeeded)
        //    {
        //        return GetErrorResult(result);
        //    }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            //if (error != null)
            //{
            //    return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            //}

            //if (!User.Identity.IsAuthenticated)
            //{
            //    return new ChallengeResult(provider, this);
            //}

            //ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            //if (externalLogin == null)
            //{
            //    return InternalServerError();
            //}

            //if (externalLogin.LoginProvider != provider)
            //{
            //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            //    return new ChallengeResult(provider, this);
            //}

            //ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
            //    externalLogin.ProviderKey));

            //bool hasRegistered = user != null;

            //if (hasRegistered)
            //{
            //    Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
            //     ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
            //        OAuthDefaults.AuthenticationType);
            //    ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
            //        CookieAuthenticationDefaults.AuthenticationType);

            //    AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
            //    Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            //}
            //else
            //{
            //    IEnumerable<Claim> claims = externalLogin.GetClaims();
            //    ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
            //    Authentication.SignIn(identity);
            //}

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // GET api/account/confirm
        [AllowAnonymous]
        [Route("Confirm", Name = "confirmation")]
        [HttpGet]
        public async Task<IHttpActionResult> Confirm(string userId, string code)
        {
            if(userId == null || code == null)
            {
                return BadRequest("Missing UserId or Token!");
            }

            var decodedCode = HttpUtility.UrlDecode(code);

            var result = await UserManager.ConfirmEmailAsync(userId, code);

            if(result.Succeeded)
            {
                return Ok(new { success = true });
            }

            return GetErrorResult(result);

        }

        [AllowAnonymous]
        [Route("ReconfirmEmail")]
        [HttpGet]
        public async Task<IHttpActionResult> ReconfirmEmail(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return BadRequest("Missing Data.");
            }
            var user = await UserManager.FindByNameAsync(userName);

            if(user == null)
            {
                return BadRequest("Missing User.");
            }

            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
            var encodedToken = HttpUtility.UrlEncode(code);

            string callback = "http://localhost:9095/confirm?userId={0}&code={1}";

            var url = string.Format(callback, user.Id, encodedToken);
            string message = "Please click this link or paste into a browser: <a href='" + url + "'>" + url + "</a>";

            await UserManager.EmailService.SendAsync(new IdentityMessage() { Subject = "Language Exchange Confirmation", Destination = user.Email, Body = message });

            return Ok();
        }

        [AllowAnonymous]
        [Route("QuickRegister")]
        public async Task<IHttpActionResult> QuickRegister(RegisterUserDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(model == null)
            {
                return BadRequest();
            }

            var user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                JoinDate = DateTime.UtcNow
            };

            await UserManager.QuickRegistration(user, model.Password);

            var ret = new
            {
                success = true
            };

            return Ok(ret);
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserDto model)
        {
            string message = "";

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new ApplicationUser()
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.Firstname,
                LastName = model.Lastname,
                JoinDate = DateTime.UtcNow
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            else
            {
                var code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                var encodedToken = HttpUtility.UrlEncode(code);

                string callback = "http://localhost:9095/confirm?userId={0}&code={1}";

                var url = string.Format(callback, user.Id, encodedToken);
                message = "Please click this link or paste into a browser: <a href='" + url + "'>" + url + "</a>";

                if (!string.IsNullOrEmpty(user.Id))
                {
                    UserManager.AddToRole(user.Id, "User");
                    UserDetail ud = (UserDetail)model;
                    ud.Id = user.Id;

                    await _userRepository.InsertUser(ud);
                    MostRecentUserDto rt = (MostRecentUserDto)model;
                    await _redis.InsertMostRecentUser(rt);
                }
            }

            await UserManager.EmailService.SendAsync(new IdentityMessage() { Subject = "You've been Registered.", Destination = user.Email, Body = message });

            var ret = new
            {
                success = true
            };

            return Ok(ret);
        }

        // POST api/Account/RegisterExternal
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
