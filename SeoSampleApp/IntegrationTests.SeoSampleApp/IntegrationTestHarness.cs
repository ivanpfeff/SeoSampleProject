using Autofac;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using SeoSampleApp.Configuration;
using SeoSampleApp.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationTests.SeoSampleApp
{
    [SetUpFixture]
    class IntegrationTestHarness
    {
        public static IContainer Container { get; set; }

        [OneTimeSetUp]
        public void SetUp()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<SearchModule>();
            containerBuilder.RegisterGeneric((ctx, t) =>
            {
                var loggerType = typeof(ILogger<>);
                var genericType = loggerType.MakeGenericType(t);
                return Substitute.For(new[] { genericType }, null);
            }).As(typeof(ILogger<>));

            //TODO: These should be pulled out into a config file
            var testSearchConfig = new SearchConfiguration()
            {
                GoogleSearchFormat = "https://www.googleapis.com/customsearch/v1?key=GOOGLEAPIKEY&cx=SEARCHENGINEID&q=SEARCH_TERM",
            };

            var testMongoConfig = new MongoConfiguration()
            {
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "SeoSampleProj_db",
            };

            containerBuilder.RegisterInstance(testSearchConfig).AsSelf();
            containerBuilder.RegisterInstance(testMongoConfig).AsSelf();

            Container = containerBuilder.Build();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Container.Dispose();
        }
    }
}
