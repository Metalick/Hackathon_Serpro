using Autofac;
using Govy.Domain.Abstract;
using Govy.Domain.Dialog;
using Govy.Domain.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Govy
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            ContainerBuilder builder = new ContainerBuilder();

            builder.RegisterType<EchoDialog>()
              .As<IDialog<object>>()
              .InstancePerDependency();

            builder.RegisterAssemblyTypes(typeof(OCRService).Assembly)
                            .Where(t => t.Name.EndsWith("Service"))
                            .AsImplementedInterfaces()
                            .SingleInstance();

            //builder.RegisterType<JobsMapper>()
            //    .Keyed<IMapper<DocumentSearchResult, GenericSearchResult>>(FiberModule.Key_DoNotSerialize)
            //    .AsImplementedInterfaces()
            //    .SingleInstance();

            //builder.RegisterType<AzureSearchClient>()
            //    .Keyed<ISearchClient>(FiberModule.Key_DoNotSerialize)
            //    .AsImplementedInterfaces()
            //    .SingleInstance();

            builder.Update(Conversation.Container);
        }
    }
}
