using FamilyManager.DataProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using FamilyManager.DataObject;
using Microsoft.AspNet.Identity.Owin;
using FamilyManager.WebApi.Models;
using System.Data.SqlClient;

namespace FamilyManager.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Family")]
    public class FamilyController : ApiController
    {
        private ApplicationUserManager _userManager;
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DbModel modelFamily = null;
        public FamilyController()
        {
            modelFamily = new DbModel();
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public async Task<IHttpActionResult> GetFamily()
        {
            try
            {
                var result = await modelFamily.GroupFamily.FirstOrDefaultAsync(x => x.Owner.UserName == RequestContext.Principal.Identity.Name);
                return GetResult(new GetFamilyReponseModel() {FamilyName = result?.Name});
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Route("AddFamily")]
        [HttpPost]
        public async Task<IHttpActionResult> AddFamily(AddFamilyBindingModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validate = model.ValidateAddFamily(modelFamily, RequestContext.Principal.Identity.Name);
                if (validate == ResponseFamilyErrorEnum.Ok)
                {
                    validate = await model.InsertFamily(modelFamily, RequestContext.Principal.Identity.Name);
                }
                return GetResult(new AddFamilyReponseModel() { ResponseCode = validate });
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Route("ModifyFamily")]
        [HttpPost]
        public async Task<IHttpActionResult> ModifyFamily(ModifyFamilyBindingModels model)
        {
            try
            {
                ResponseFamilyErrorEnum responseCode = ResponseFamilyErrorEnum.Ok;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validate = model.ValidateFamily(modelFamily, RequestContext.Principal.Identity.Name);
                if (validate != null)
                {
                    validate.Name = model.FamilyName;
                    responseCode = await model.MofifyFamily(modelFamily, validate);
                }
                else
                    responseCode = ResponseFamilyErrorEnum.UserNotFamilyOwner;

                return GetResult(new AddFamilyReponseModel() { ResponseCode = responseCode });
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Route("SendInvitationtoFamily")]
        [HttpPost]
        public IHttpActionResult SendInvitationtoFamily(FamilyInvitationModels model)
        {
            try
            {
                var family = model.ValidateFamily(modelFamily, RequestContext.Principal.Identity.Name);
                if (family != null)
                {
                    model.SendtoMail(model.Email, family.Name);
                    return GetResult(new ResponseModel() { ResponseCode = ResponseFamilyErrorEnum.Ok});
                }
                else
                    return GetResult(new InvitationReponseModel() { ResponseCode = ResponseFamilyErrorEnum.UserNotFamilyOwner });
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Route("ConfirmUsertoFamily")]
        [HttpPost]
        public async Task<IHttpActionResult> ConfirmUsertoFamily(ConfirmUserFamilyBindingModels model)
        {
            try
            {
                ResponseFamilyErrorEnum responseCode = ResponseFamilyErrorEnum.Ok;
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                responseCode = model.ValidateFamily(modelFamily, RequestContext.Principal.Identity.Name, model.FamilyName);
                if (responseCode == ResponseFamilyErrorEnum.Ok)
                {
                    responseCode = await model.AddUsertoFamily(modelFamily, RequestContext.Principal.Identity.Name,
                        model.FamilyName);
                }

                return GetResult(new ResponseModel() { ResponseCode = responseCode });
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        [Route("ExitUsertoFamily")]
        [HttpPost]
        public async Task<IHttpActionResult> ExitUsertoFamily()
        {
            try
            {
                ResponseFamilyErrorEnum responseCode = ResponseFamilyErrorEnum.Ok;
                ExitUsertoFamilyBindingModels model = new ExitUsertoFamilyBindingModels();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var validate = model.ValidateFamily(modelFamily, RequestContext.Principal.Identity.Name);
                if (validate != null)
                {
                    responseCode = await model.ExitUsertoFamily(modelFamily, validate);
                }

                return GetResult(new ResponseModel() { ResponseCode = responseCode });
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return BadRequest(ex.Message);
            }
        }
        private IHttpActionResult GetResult(ResponseModel result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.ResponseCode != ResponseFamilyErrorEnum.Ok)
            {
                ModelState.AddModelError(result.GetPropertyError(result.ResponseCode), result.Messagge);
                if (ModelState.IsValid)
                {
                    // No hay disponibles errores ModelState para enviar, por lo que simplemente devuelva un BadRequest vacío.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }else
                return Ok(result);
        }
  
    }
}
