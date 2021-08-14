using Autofac;
using SeoSampleApp.Services;
using SeoSampleApp.Services.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }
    }
}
