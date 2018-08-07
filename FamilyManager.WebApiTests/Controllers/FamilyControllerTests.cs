using Microsoft.VisualStudio.TestTools.UnitTesting;
using FamilyManager.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net.Http;
using System.Web.Http.Results;
using System.Web.Routing;
using System.Security.Principal;
using System.Threading;

namespace FamilyManager.WebApi.Controllers.ControllerTests
{
    [TestClass()]
    public class FamilyControllerTests
    {
        [TestMethod()]
        public void AddFamilyTest()
        {
            var identity = new GenericIdentity("psilva@geocom.com.uy");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            FamilyController controller = new FamilyController();
            var task = controller.AddFamily(new Models.AddFamilyBindingModels()
            {
                FamilyName = "Silva"
            });
            task.Wait();
            IHttpActionResult actionResult = task.Result;
           Assert.IsInstanceOfType(actionResult,typeof(OkResult));
        }
    }
}