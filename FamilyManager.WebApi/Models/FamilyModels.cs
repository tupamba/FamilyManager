using FamilyManager.DataObject;
using FamilyManager.DataProvider;
using FamilyManager.Repository;
using FamilyManager.WebApi.Models;
using FamilyManager.WebApi.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FamilyManager.WebApi.Models
{
    public class FamilyModels
    {
       

    }
    public class AddFamilyBindingModels //: IValidatableObject
    {
        [Required(ErrorMessage = "Nombre es un dato obligatorio")]
        public string FamilyName { get; set; }
        public GroupFamily GetGroupFamily()
        {
            return new GroupFamily()
            { Name = FamilyName};
        }
        public async Task<ResponseFamilyErrorEnum> ValidateAddFamily(IRepositoryBase<GroupFamily> modelFamily, string user)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add("Name", FamilyName);
            data.Add("Owner", user);
            var result = await modelFamily.QueryExecuteAsync<int>("SelectFamily @Name, @Owner", data);
            return (ResponseFamilyErrorEnum)result.FirstOrDefault();
            //SqlParameter param1 = new SqlParameter("@Name", FamilyName);
            //SqlParameter param2 = new SqlParameter("@Owner", user);
            //return (ResponseFamilyErrorEnum)modelFamily.
            //    Database.SqlQuery<int>("SelectFamily @Name, @Owner", param1, param2).FirstOrDefault();
        }
        public async Task<ResponseFamilyErrorEnum> InsertFamily(IRepositoryBase<GroupFamily> modelFamily, IRepositoryBase<MemberFamily> memberdb, string user)
        {
            try
            {
                //var member = await memberdb.FindByConditionAync(x => x.UserName == user);
                //var userMember = member.DefaultIfEmpty(new MemberFamily(user)).FirstOrDefault();
                //modelFamily.Create(
                //            new GroupFamily(new List<MemberFamily>()
                //            { userMember
                //            }, FamilyName, userMember));
                var result = await modelFamily.SaveAsync();
                if (result != 0)
                    return ResponseFamilyErrorEnum.Ok;
                else
                    return ResponseFamilyErrorEnum.Error;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        //public async Task<ResponseFamilyErrorEnum> InsertFamily(DbModel modelFamily, string user)
        //{
        //    var member = modelFamily.MemberFamily.FirstOrDefault(x => x.UserName == user);
        //    var userMember = member == null ? new MemberFamily(user) : member;
        //    modelFamily.GroupFamily.Add(
        //                new GroupFamily(new List<MemberFamily>()
        //                { userMember
        //                }, FamilyName, userMember));
        //    var result = await modelFamily.SaveChangesAsync();
        //    if (result != 0)
        //        return ResponseFamilyErrorEnum.Ok;
        //    else
        //        return ResponseFamilyErrorEnum.Error;
        //}

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    validationContext.GetService(typeof(IExternalService));
        //    if (FamilyName == "Prueba")
        //        yield return new ValidationResult("Error de prueba", new List<string>() { "FamilyName" });
        //}
    }
    public class ModifyFamilyBindingModels 
    {
        [Required(ErrorMessage = "Nombre es un dato obligatorio")]
        public string FamilyName { get; set; }

        //public GroupFamily ValidateFamily(DbModel modelFamily, string user)
        //{
        //    var result = modelFamily.GroupFamily.FirstOrDefault(x => x.Owner.UserName == user);
        //    return result;
        //}
        public async Task<ResponseFamilyErrorEnum> MofifyFamily(DbModel modelFamily, GroupFamily family)
        {
            var result = await modelFamily.SaveChangesAsync();
            return ResponseFamilyErrorEnum.Ok;
        }

    }
    public class ConfirmUserFamilyBindingModels
    {
        [Required(ErrorMessage = "Nombre es un dato obligatorio")]
        public string FamilyName { get; set; }

        public ResponseFamilyErrorEnum ValidateFamily(DbModel modelFamily, string user, string familyName)
        {
            var result = modelFamily.MemberFamily.Any(x => x.UserName == user);
            if (result)
                result = modelFamily.GroupFamily.Any(x => x.MembersFamily.Any(y => y.UserName == user));
            if (!result)
                result = modelFamily.GroupFamily.Any(x => x.Name == familyName);
            else
                return ResponseFamilyErrorEnum.UserAlredyExist;

            return !result? ResponseFamilyErrorEnum.FamilyNameNotExist : ResponseFamilyErrorEnum.Ok;
        }
        public async Task<ResponseFamilyErrorEnum> AddUsertoFamily(DbModel modelFamily, string user, string familyName)
        {
            var userModel = modelFamily.MemberFamily.FirstOrDefault(x => x.UserName == user);
            modelFamily.GroupFamily.FirstOrDefault(x => x.Name == familyName)?.MembersFamily.Add
                (userModel == null?new MemberFamily(user, false): userModel);
            var result = await modelFamily.SaveChangesAsync();
            if (result != 0)
                return ResponseFamilyErrorEnum.Ok;
            else
                return ResponseFamilyErrorEnum.Error;
        }

    }
    public class ExitUsertoFamilyBindingModels
    {
        public MemberFamily ValidateFamily(DbModel modelFamily, string user)
        {
            var result = modelFamily.GroupFamily.Any(x => x.Name == user || !x.MembersFamily.Any(y => y.UserName == user));
            if (result)
                return null;
            else
                return modelFamily.MemberFamily.FirstOrDefault(x => x.UserName == user);
        }
        public async Task<ResponseFamilyErrorEnum> ExitUsertoFamily(DbModel modelFamily, MemberFamily member)
        {
            var family = modelFamily.GroupFamily.Include("MembersFamily").FirstOrDefault(x => 
            x.MembersFamily.Any(y => y.UserName == member.UserName));
            family?.MembersFamily.Remove
                (member);
            var result = await modelFamily.SaveChangesAsync();
            if (result != 0)
                return ResponseFamilyErrorEnum.Ok;
            else
                return ResponseFamilyErrorEnum.Error;
        }

    }
    public class FamilyInvitationModels
    {
        public string Email { get; set; }
        public GroupFamily ValidateFamily(DbModel modelFamily, string user)
        {
            var result = modelFamily.GroupFamily.FirstOrDefault(x => x.Name == user);
            return result;
        }
        public void SendtoMail(string tosend, string family)
        {
            StringBuilder text = new StringBuilder();
            text.AppendLine("La familia " + family + " te a envíado una invitación para compartir esta aplicación");
            text.AppendLine("Ingresa en el siguiente link para confirmar la invitación, registrandote en el sistema si aún no lo has hecho");
            text.AppendLine("Link: " + ConfigurationManager.AppSettings["WebApp"] + "confirmInvitation?family=" + family);
            string subject = "La familia " + family + " te a envíado una invitación";
            string body = "Body";
            SmtpMailService.SendtoMail(tosend, subject, body); 
        }

    }

    #region Response
    public class AddFamilyReponseModel : ResponseModel
    {
        //public override string GetMessagge(ResponseFamilyErrorEnum code)
        //{
        //    return GetMessaggeError(code);
        //}
        public string GetPropertyError(int error)
        {
            switch (error)
            {
                case (int)ResponseFamilyErrorEnum.FamilyNameDuplicate:
                    return "FamilyName";
                default:
                    return "";
            }
        }
        public string GetMessaggeError(ResponseFamilyErrorEnum error)
        {
            switch (error)
            {
                case ResponseFamilyErrorEnum.UserOwnerFamilyAlredyExist:
                    return "User alredy has a family group";
                case ResponseFamilyErrorEnum.FamilyNameDuplicate:
                    return "Family name duplicate";
                default:
                    return "";
            }
        }
    }
    public class InvitationReponseModel : ResponseModel
    {
        //public override string GetMessagge(int code)
        //{
        //    return GetMessaggeError(code);
        //}
        
        public string GetMessaggeError(int error)
        {
            switch (error)
            {
                case (int)ResponseFamilyErrorEnum.SendMailFail:
                    return "Send mail fail";
                case (int)ResponseFamilyErrorEnum.UserNotFamilyOwner:
                    return "User not family owner";
                default:
                    return "";
            }
        }
       
    }
    public class GetFamilyReponseModel : ResponseModel
    {
        public string FamilyName { get; set; }
    }
    #endregion
    public enum ResponseFamilyErrorEnum
    {
        Ok,
        Error,
        SendMailFail,
        UserNotFamilyOwner,
        UserOwnerFamilyAlredyExist,
        FamilyNameDuplicate,
        UserAlredyExist,
        FamilyNameNotExist,
        UserFamilyAlredyExist,
        Excpetion
    }
}