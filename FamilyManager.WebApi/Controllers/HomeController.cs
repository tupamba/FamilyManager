using FamilyManager.WebApi.Command;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FamilyManager.WebApi.Controllers
{
    public class HomeController : Controller
    {
       // private readonly IMediator _mediator;
        public HomeController()
        {
          //  _mediator = mediator;

        }
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
           // AddGroupFamilyCommand command = new AddGroupFamilyCommand(null, "tupamba@gmail.com");
           // _mediator.Send(command).Wait();
            return View();
        }
    }
}
