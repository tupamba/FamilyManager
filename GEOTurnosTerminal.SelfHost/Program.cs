using FamilyManager.WebApi;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace FamilyManager.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            const string baseAddress = "http://localhost:9123/";

            // This starts the OWIN host using the application startup
            // logic in the Startup class. See Startup for the example of
            // how to set up OWIN Web API.
            using (WebApp.Start<StartupAutofac>(baseAddress))
            {
                // On startup this app will make a request to the self-hosted
                // Web API service. You should see logging statements and results
                // dumped to the console window.
                var client = new HttpClient();
                var response = client.GetAsync(baseAddress + "api/Family/GetFamily").Result;

                Console.WriteLine(response);
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                Console.ReadLine();
            }

            if (Debugger.IsAttached)
            {
                Console.WriteLine("Press any key to exit.");
                Console.ReadLine();
            }
        }
    }
}
