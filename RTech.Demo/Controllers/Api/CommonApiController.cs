using Riddhasoft.Employee.Services;
using Riddhasoft.User.Entity;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace RTech.Demo.Controllers.Api
{
    public class CommonApiController : ApiController
    {
        [HttpGet]
        public string ConvertDate(string value, string dateType)
        {
            try
            {
                SDateTable dateService = new SDateTable();
                switch (dateType)
                {
                    case "AD": return dateService.ConvertToEngDate(value).ToString("yyyy/MM/dd");
                    case "BS": return dateService.ConvertToNepDate(value.ToDateTime());
                    default:
                        return "";
                }
            }
            catch
            {

                return "";
            }
        }

        // GET api/commonapi/5
        public string GetLocaliseStringRequired(string field)
        {
            LocalizedString l = new LocalizedString();
            return l.Localize(field) + " " + l.Localize("RequiredValidation");
        }
        public string GetLocaliseString(string field)
        {
            LocalizedString l = new LocalizedString();
            return l.Localize(field);
        }
        [HttpGet]
        public string GetCurrentToken()
        {
            return RiddhaSession.CurrentToken;
        }
        [HttpGet]
        public string GetCurrentLanguage()
        {
            return RiddhaSession.Language;
        }
        [HttpGet]
        public string GetCurrentOperationDate()
        {
            return RiddhaSession.Language;
        }
        [HttpGet]
        public void SetCurrentOpDate(string opDate)
        {
            RiddhaSession.OperationDate = opDate;
        }
        [HttpGet]
        public void SetCurrentLanguage(string lang)
        {
            RiddhaSession.Language = lang;
        }
        [HttpGet]
        public string GetCurrentUserName()
            {
            return RiddhaSession.CurrentUser.Name;
        }
        // POST api/commonapi
        public void Post([FromBody]string value)
        {
        }

        // PUT api/commonapi/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/commonapi/5
        public void Delete(int id)
        {
        }
    }
}
