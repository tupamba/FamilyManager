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
using MediatR;
using Moq;
using Autofac.Extras.Moq;
using FamilyManager.WebApi.Command.FamilyController;
using FamilyManager.WebApi.Models;
using Autofac;
using FamilyManager.Repository;
using FamilyManager.WebApi.QueryObject;
using System.Data.Entity;
using FamilyManager.DataObject;
using FamilyManager.DataProvider;
using System.Data.Entity.Infrastructure;

namespace FamilyManager.WebApi.Controllers.ControllerTests
{

    [TestClass()]
    public class FamilyControllerTests 
    {
        [TestMethod()]
        public void AddFamilyTest()
        {
    
            var mediator = new Mock<IMediator>();
            mediator
    .Setup(m => m.Send(It.IsAny<AddGroupFamilyCommand>(), It.IsAny<CancellationToken>()))
    .ReturnsAsync(ResponseFamilyErrorEnum.Ok) //<-- return Task to allow await to continue
    .Verifiable("Notification was not sent.");
            var identity = new GenericIdentity("psilva@geocom.com.uy");
            Thread.CurrentPrincipal = new GenericPrincipal(identity, null);
            FamilyController controller = new FamilyController(mediator.Object, null);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();
           var task = controller.AddFamily(new Models.AddFamilyBindingModels()
            {
                FamilyName = "Silva"
            });
            task.Wait();
            var actionResult = task.Result.ExecuteAsync(new CancellationToken()).Result;
            ModelResult model;
            var contentResult = actionResult.TryGetContentValue<ModelResult>(out model);
            Assert.IsTrue(actionResult.IsSuccessStatusCode, model.Message);
        }
    }
}