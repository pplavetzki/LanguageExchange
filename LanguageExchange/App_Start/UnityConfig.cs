using LanguageExchange.Interfaces;
using LanguageExchange.Models;
using LanguageExchange.Providers;
using LanguageExchange.Repository;
using LanguageExchange.Security;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Practices.Unity;
using StackExchange.Redis;
using System;
using System.Configuration;
using System.Web.Http;
using Unity.WebApi;

namespace LanguageExchange
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            var documentUri = ConfigurationManager.AppSettings["DocumentUri"];
            string authKey = ConfigurationManager.AppSettings["AuthorizationKey"];
            string redisConnection = ConfigurationManager.AppSettings["RedisConnection"];

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            Uri uri = new Uri(documentUri);
            ConnectionPolicy connectionPolicy = new ConnectionPolicy() { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp };
            ConsistencyLevel consistencyLevel = new ConsistencyLevel();
            consistencyLevel = ConsistencyLevel.Session;

            container.RegisterType<IApplicationUserStore, ApplicatonUserStore>();
            container.RegisterType<DocumentClient>(new ContainerControlledLifetimeManager(), new InjectionConstructor(uri, authKey, connectionPolicy, consistencyLevel));

            ConnectionMultiplexer connectionMulp = ConnectionMultiplexer.Connect(redisConnection);
            container.RegisterInstance<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConnection));

            container.RegisterType<IRedisRepository, RedisRepository>();
            container.RegisterType<IUserRepository, UserRepository>();

            var repo = container.Resolve<IRedisRepository>();

            container.RegisterType<RefreshTokenProvider>(new InjectionConstructor(repo));

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}