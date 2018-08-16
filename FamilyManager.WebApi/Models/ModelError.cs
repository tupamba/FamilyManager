using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FamilyManager.WebApi.Models
{
    public class ActionResult : IHttpActionResult
    {
        ModelResult _value;
        HttpRequestMessage _request;

        public ActionResult(ModelResult value, HttpRequestMessage request)
        {
            try
            {
                _value = value;
                _request = request;
            }
            catch (Exception)
            {
            }

        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;
            try
            {
                if (_value == null || _value.ModelState == ResponseFamilyErrorEnum.Excpetion)
                    response = _request.CreateResponse(HttpStatusCode.InternalServerError);
                else if (_value.ModelState == ResponseFamilyErrorEnum.Ok)
                    response = _request.CreateResponse(HttpStatusCode.BadRequest, _value);
                else
                    response = _request.CreateResponse(HttpStatusCode.BadRequest, _value);
            }
            catch (Exception)
            {
                response = _request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return Task.FromResult(response);
        }
    }
    public class ModelResult
    {
        public string Message { get; set; }
        public ResponseFamilyErrorEnum ModelState { get; set; }
        public static ActionResult GetResult(string message, ResponseFamilyErrorEnum codeResult,HttpRequestMessage request)
        {
            var result = new ModelResult()
            {
                Message = message,
                ModelState = codeResult
            };

            return new ActionResult(result, request);
        }
    }
}