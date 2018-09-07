using FamilyManager.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FamilyManager.WebApi.Controllers
{
    public class ControllerBase : ApiController
    {
        public static IHttpActionResult GetResult(ResponseModel result, HttpRequestMessage request, object data = null)
        {
            return ModelResult.GetResult(result.Messagge, result.ResponseCode, request, result);
        }
    }
}