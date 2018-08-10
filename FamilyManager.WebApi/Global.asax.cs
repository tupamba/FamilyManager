using Autofac;
using Autofac.Integration.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using FamilyManager.Repository;
using FamilyManager.WebApi.Controllers;
using FamilyManager.DataProvider;
using System.Data.Entity;
using FamilyManager.WebApi.Command;
using Autofac.Features.Variance;
using MediatR;
using FamilyManager.WebApi.QueryObject;

namespace FamilyManager.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));

            var builder = new ContainerBuilder();

            var config = GlobalConfiguration.Configuration;

            builder.RegisterControllers(typeof(WebApiApplication).Assembly);
            builder.RegisterApiControllers(typeof(WebApiApplication).Assembly);

            builder.RegisterType(typeof(DbModel)).As(typeof(DbContext)).InstancePerLifetimeScope();
            builder.RegisterType<GroupFamilyRepository>().AsImplementedInterfaces();
            builder.RegisterType<QueryFactory>().AsImplementedInterfaces();
            
            builder
              .RegisterType<Mediator>()
              .As<IMediator>()
              .InstancePerLifetimeScope();

            object o = null;
            // request handlers
            builder
              .Register<ServiceFactory>(ctx => {
                  var c = ctx.Resolve<IComponentContext>();
                  return t => c.TryResolve(t,out o) ? o : null;
              })
              .InstancePerLifetimeScope();

            // notification handlers
            //builder
            //  .Register<ServiceFactory>(ctx => {
            //      var c = ctx.Resolve<IComponentContext>();
            //      return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            //  })
            //  .InstancePerLifetimeScope();

            // finally register our custom code (individually, or via assembly scanning)
            // - requests & handlers as transient, i.e. InstancePerDependency()
            // - pre/post-processors as scoped/per-request, i.e. InstancePerLifetimeScope()
            // - behaviors as transient, i.e. InstancePerDependency()
            builder.RegisterAssemblyTypes(typeof(WebApiApplication).GetTypeInfo().Assembly).AsImplementedInterfaces(); // via assembly scan

            //builder.RegisterType<MyHandler>().AsImplementedInterfaces().InstancePerDependency();  

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}
