using DevExpress.Xpo.Metadata;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Xpo.Entities;
using IdentityServer4.Xpo.Services;
using IdentityServer4.Xpo.Stores;
using IdentityServer4.Xpo.TokenCleanup;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace IdentityServer4.Xpo.Extensions
{
    public static class IdentityServerBuilderExtensions
    {
        /// <summary>
        /// Registers the IdentityServer stores with the container
        /// If the <paramref name="connectionString"> is passed , a <see name="IDocumentStore"> and <see name="IDocumentSession"> are registered
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="connectionString">A Xpo connectionstring</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddXpo(
        this IIdentityServerBuilder builder, string connectionString = null, params Assembly[] xpoAssemblys)
        {
            builder.Services.AddTransient<IClientStore, ClientStore>();
            builder.Services.AddTransient<IResourceStore, ResourceStore>();
            builder.Services.AddTransient<ICorsPolicyService, CorsPolicyService>();

            if (!string.IsNullOrEmpty(connectionString))
            {
                builder.Services
                .AddXpoDefaultUnitOfWork(true, options => options
                    .UseConnectionString(connectionString)
                    .UseThreadSafeDataLayer(true)
                    //.UseConnectionPool(false) // Remove this line if you use a network database like SQL Server, Oracle, PostgreSql etc.                    
                    .UseAutoCreationOption(DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema) // Remove this line if the database already exists
                                                                                                 //.UseEntityTypes(typeof(Customer), typeof(Order)) // Pass all of your persistent object types to this method.
                    .UseCustomDictionaryFactory(() =>
                    {
                        DevExpress.Xpo.Metadata.XPDictionary dictionary = new ReflectionDictionary();
                        //DevExpress.Xpo.Metadata.ReflectionClassInfo.SuppressSuspiciousMemberInheritanceCheck = true;

                        var assemblys = new List<Assembly>
                        {
                            typeof(ApiResource).Assembly
                        };
                        if (xpoAssemblys != null)
                        {
                            assemblys.AddRange(xpoAssemblys);
                        }
                        dictionary.GetDataStoreSchema(assemblys.ToArray());
                        //DevExpress.Xpo.DB.IDataStore store = DevExpress.Xpo.XpoDefault.GetConnectionProvider(conn, DevExpress.Xpo.DB.AutoCreateOption.SchemaOnly);
                        DevExpress.Xpo.DB.IDataStore store = DevExpress.Xpo.XpoDefault.GetConnectionProvider(connectionString, DevExpress.Xpo.DB.AutoCreateOption.DatabaseAndSchema);
                        DevExpress.Xpo.XpoDefault.DataLayer = new DevExpress.Xpo.ThreadSafeDataLayer(dictionary, store);
                        DevExpress.Xpo.XpoDefault.Session = new DevExpress.Xpo.Session(DevExpress.Xpo.XpoDefault.DataLayer);
                        return dictionary;
                    })
                );
            }

            return builder;
        }

        public static IIdentityServerBuilder AddConfigurationStoreCache(
           this IIdentityServerBuilder builder)
        {
            builder.AddInMemoryCaching();

            // these need to be registered as concrete classes in DI for
            // the caching decorators to work
            builder.Services.AddTransient<ClientStore>();
            builder.Services.AddTransient<ResourceStore>();

            // add the caching decorators
            builder.AddClientStoreCache<ClientStore>();
            builder.AddResourceStoreCache<ResourceStore>();

            return builder;
        }

        public static IIdentityServerBuilder AddOperationalStore(this IIdentityServerBuilder builder, Action<TokenCleanupOptions> tokenCleanUpOptions = null)
        {
            builder.Services.AddScoped<IPersistedGrantStore, PersistedGrantStore>();
            var tokenCleanupOptions = new TokenCleanupOptions();
            tokenCleanUpOptions?.Invoke(tokenCleanupOptions);
            builder.Services.AddSingleton(tokenCleanupOptions);
            builder.Services.AddSingleton<TokenCleanup.TokenCleanup>();
            return builder;
        }
        public static IApplicationBuilder UseIdentityServerTokenCleanup(this IApplicationBuilder app, IApplicationLifetime applicationLifetime)
        {
            var tokenCleanup = app.ApplicationServices.GetService<TokenCleanup.TokenCleanup>();
            if (tokenCleanup == null)
            {
                throw new InvalidOperationException("AddOperationalStore() must be called before calling this method.");
            }
            applicationLifetime.ApplicationStarted.Register(tokenCleanup.Start);
            applicationLifetime.ApplicationStopping.Register(tokenCleanup.Stop);

            return app;
        }


    }
}
