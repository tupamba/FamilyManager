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
using FamilyManager.Repository;
using System.Reflection;
using FamilyManager.WebApi.Command;
using MediatR;
using FamilyManager.WebApi.QueryObject;
using FamilyManager.WebApi.Command.FamilyController;
using Newtonsoft.Json;

namespace FamilyManager.WebApi.Controllers
{
    [Authorize]
    [RoutePrefix("api/Family")]
    public class FamilyController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IQueryFactory _factoryQuery;
        private ApplicationUserManager _userManager;
        readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DbModel modelFamily = null;
        public FamilyController(IMediator mediator, IQueryFactory factory)
        {
            _mediator = mediator;
            _factoryQuery = factory;

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
                var result = await _factoryQuery.GetGroupFamilyQuery().GetFamilyUser(RequestContext.Principal.Identity.Name);
                return GetResult(new GetFamilyReponseModel() {FamilyName = result?.Name});
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return GetExcpetion(ex.Message);
            }
        }
        /// <summary>
        /// Add new group family
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                AddGroupFamilyCommand command = new AddGroupFamilyCommand(model.GetGroupFamily(), RequestContext.Principal.Identity.Name);
                var result = await _mediator.Send(command);
                return GetResult(new AddFamilyReponseModel() { ResponseCode = result },Request);
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return GetExcpetion(ex.Message);
            }

        }
        /// <summary>
        /// Modify name this group family
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("ModifyFamily")]
        [HttpPost]
        public async Task<IHttpActionResult> ModifyFamilyName(ModifyFamilyBindingModels model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                NameModifyGroupFamilyCommand command = new NameModifyGroupFamilyCommand(
                    model.FamilyName, RequestContext.Principal.Identity.Name);
                var result = await _mediator.Send(command);

                return GetResult(new AddFamilyReponseModel() { ResponseCode = result });
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return GetExcpetion(ex.Message);
            }
        }
        /// <summary>
        /// Send invitation to add group family. The invitation send for mail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                return GetExcpetion(ex.Message);
            }
        }
        /// <summary>
        /// Request firendship to add group family
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("SendRequestFriendship")]
        [HttpPost]
        public IHttpActionResult SendRequestFriendship(FamilyInvitationModels model)
        {
            try
            {
                var family = model.ValidateFamily(modelFamily, RequestContext.Principal.Identity.Name);
                if (family != null)
                {
                    model.SendtoMail(model.Email, family.Name);
                    return GetResult(new ResponseModel() { ResponseCode = ResponseFamilyErrorEnum.Ok });
                }
                else
                    return GetResult(new InvitationReponseModel() { ResponseCode = ResponseFamilyErrorEnum.UserNotFamilyOwner });
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return GetExcpetion(ex.Message);
            }
        }
        /// <summary>
        /// Get out user off group family
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("GetOutofftheGroup")]
        [HttpPost]
        public IHttpActionResult GetOutofftheGroup(FamilyInvitationModels model)
        {
            try
            {
                var family = model.ValidateFamily(modelFamily, RequestContext.Principal.Identity.Name);
                if (family != null)
                {
                    model.SendtoMail(model.Email, family.Name);
                    return GetResult(new ResponseModel() { ResponseCode = ResponseFamilyErrorEnum.Ok });
                }
                else
                    return GetResult(new InvitationReponseModel() { ResponseCode = ResponseFamilyErrorEnum.UserNotFamilyOwner });
            }
            catch (Exception ex)
            {
                log.Fatal(ex.Message, ex);
                return GetExcpetion(ex.Message);
            }
        }
        /// <summary>
        /// Confirm add to group family from invitation
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
                return GetExcpetion(ex.Message);
            }
        }
        /// <summary>
        /// Owner Acept request user to add group family
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("AceptUsertoFamily")]
        [HttpPost]
        public async Task<IHttpActionResult> AceptUsertoFamily(ConfirmUserFamilyBindingModels model)
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
                return GetExcpetion(ex.Message);
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
                return GetExcpetion(ex.Message);
            }
        }
        private IHttpActionResult GetResult(ResponseModel result)
        {
             return GetResult(result, Request);
            
        }
        private IHttpActionResult GetExcpetion(string message)
        {
            var result = new ResponseModel()
            {
                ResponseCode = ResponseFamilyErrorEnum.Excpetion,
                Messagge = message
            };
            return GetResult(result, Request);
        }
    }
}
