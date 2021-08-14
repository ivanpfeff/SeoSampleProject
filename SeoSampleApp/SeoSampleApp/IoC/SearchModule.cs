using Autofac;
using MongoDB.Driver;
using SeoSampleApp.Configuration;
using SeoSampleApp.Services;
using SeoSampleApp.Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;

namespace SeoSampleApp.IoC
{
    public class SearchModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            //Register search related components
            builder.RegisterType<SearchService>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<SearchHistoryService>().AsImplementedInterfaces().InstancePerDependency();

            builder.Register(ctx =>
            {
                var mongoConfig = ctx.Resolve<MongoConfiguration>();
                var connectionString = mongoConfig.ConnectionString;

                MongoClientSettings settings = MongoClientSettings.FromUrl(
                  new MongoUrl(connectionString)
                );

                settings.SslSettings =
                  new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };

                var client = new MongoClient(settings);
                var database = client.GetDatabase(mongoConfig.DatabaseName);
                return database;
            }).As<IMongoDatabase>().SingleInstance();
        }
    }
}
