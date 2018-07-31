using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FamilyManager.WebApi.Models
{
    public class ResponseModel
    {
        private ResponseFamilyErrorEnum responseCode;

        public ResponseFamilyErrorEnum ResponseCode
        {
            get { return responseCode; }
            set {  responseCode = value; Messagge = GetMessagge(responseCode); }
        }
        public string Messagge { get; set; }
        public virtual string GetMessagge(ResponseFamilyErrorEnum code)
        {
            switch (code)
            {
                case ResponseFamilyErrorEnum.UserOwnerFamilyAlredyExist:
                    return "User alredy has a family group";
                case ResponseFamilyErrorEnum.FamilyNameDuplicate:
                    return "Family name duplicate";
                case ResponseFamilyErrorEnum.UserAlredyExist:
                    return "User alredy exist";
                case ResponseFamilyErrorEnum.FamilyNameNotExist:
                    return "Family name not exist";
                default:
                    return "";
            }
        }
        public virtual string GetPropertyError(ResponseFamilyErrorEnum code)
        {
            switch (code)
            {
                case ResponseFamilyErrorEnum.UserAlredyExist:
                case ResponseFamilyErrorEnum.FamilyNameDuplicate:
                    return "FamilyName";
                default:
                    return "";
            }
        }
    }
   
}