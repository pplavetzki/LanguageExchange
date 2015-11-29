using LanguageExchange.Interfaces;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;
using LanguageExchange.Models.Dtos;
using LanguageExchange.Common;

namespace LanguageExchange.Providers
{
    public class RefreshTokenProvider : IAuthenticationTokenProvider
    {
        private readonly IRedisRepository _redisRepo;

        public RefreshTokenProvider()
        {
            IDependencyResolver container = GlobalConfiguration.Configuration.DependencyResolver;
            _redisRepo = (IRedisRepository)container.GetService(typeof(IRedisRepository));
        }

        public RefreshTokenProvider(IRedisRepository redisRepository)
        {
            _redisRepo = redisRepository;
        }
        
        public void Create(AuthenticationTokenCreateContext context)
        {
            CreateAsync(context).Wait();
        }

        public async Task CreateAsync(AuthenticationTokenCreateContext context)
        {
            var clientid = context.Ticket.Properties.Dictionary["as:client_id"];
            var refreshTokenLifetime = context.OwinContext.Get<string>("as:RefreshTokenLifetime");

            var refreshTokenId = Guid.NewGuid().ToString("n");

            if (string.IsNullOrEmpty(clientid))
            {
                return;
            }

            context.Ticket.Properties.IssuedUtc = DateTime.UtcNow;
            context.Ticket.Properties.ExpiresUtc = DateTime.UtcNow.AddMinutes(Convert.ToDouble(refreshTokenLifetime));
            var token = context.SerializeTicket();

            var refreshDto = new RefreshTokenDto()
            {
                Id = refreshTokenId,
                ClientId = clientid,
                Subject = context.Ticket.Identity.Name,
                Token = token
            };

            await _redisRepo.InsertRefreshTokenAsync(refreshDto);

            context.SetToken(refreshTokenId);
        }

        public void Receive(AuthenticationTokenReceiveContext context)
        {
            ReceiveAsync(context).Wait();
        }

        public async Task ReceiveAsync(AuthenticationTokenReceiveContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:AllowedOrigin");
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });

            string tokenId = context.Token;

            var refreshToken = await _redisRepo.GetRefreshTokenAsync(tokenId);

            if (refreshToken != null)
            {
                context.DeserializeTicket(refreshToken.Token);
                var result = await _redisRepo.RemoveRefreshToken(tokenId, refreshToken.Subject);
            }
            
        }
    }
}
