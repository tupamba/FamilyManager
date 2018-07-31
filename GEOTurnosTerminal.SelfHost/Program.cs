using FamilyManager.WebSite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FamilyManager.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(factory =>
            {
                // Provide the service's behavior using our custom
                //  ServiceHost class
                //
                factory.Service<IServiceHost>(service =>
                {
                    service.ConstructUsing(name => ServiceHostCreator.GetServiceHost(ServiceHostEnum.ServiceHost));
                    service.WhenStarted(sh => sh.Start());
                    service.WhenShutdown(sh => sh.Shutdown());
                    service.WhenStopped(sh => sh.Stop());
                }); 

                // Now define some attributes of the service overall
                //
                factory.RunAsLocalSystem();

                // Provide the metadata to the service control
                //
                factory.SetServiceName("self-hosted-angular2-service");
                factory.SetDisplayName("Self-hosted Angular 2 service");
                factory.SetDescription("A custom service that hosts an Angular 2 website using OWIN");

            });
        }
    }
}
