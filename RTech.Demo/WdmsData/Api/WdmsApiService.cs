using Newtonsoft.Json;
using Riddhasoft.Device.Entities;
using Riddhasoft.Device.Services;
using RTech.Demo.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace RTech.Demo.WdmsData.Api
{
    public class WdmsApiService
    {
        CookieContainer cookies = new CookieContainer();
        HttpClientHandler handler = new HttpClientHandler();
        HttpClient client;
        string Url = "";
        string Username = "";
        string Password = "";
        public string Token = "";
        public WdmsApiService()
        {
            SWdmsConfig service = new SWdmsConfig();
            EWdmsConfig obj = service.Get().Data;
            this.Url = obj.Url;
            this.Username = obj.UserName;
            this.Password = obj.Password;
            Login();
        }

        public void Login()
        {
            handler.CookieContainer = cookies;
            client = new HttpClient(handler);
            var uri = new Uri(Url);
            client.BaseAddress = new Uri(Url);
            LoginModel login = new LoginModel()
            {
                username = Username,
                password = Password
            };
            try
            {
                HttpResponseMessage response = client.PostAsJsonAsync("api/accounts/login/ ", login).Result;
                response.EnsureSuccessStatusCode();
                IEnumerable<Cookie> responseCookies = cookies.GetCookies(uri).Cast<Cookie>();
                var sessionid = responseCookies.Where(x => x.Name == "sessionid").First().Value;
                Token = sessionid;
            }
            catch (Exception)
            {

                return;
            }

        }
        //static async Task<Product> GetProductAsync(string path)
        //{
        //    Product product = null;
        //    HttpResponseMessage response = await client.GetAsync(path);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        product = await response.Content.ReadAsAsync<Product>();
        //    }
        //    return product;
        //}

        internal bool UpdateEmployee(Riddhasoft.Employee.Services.EmployeeVm model)
        {
            //CookieContainer cookies = new CookieContainer();
            //HttpClientHandler handler = new HttpClientHandler();
            //HttpClient client;
            //handler.CookieContainer = cookies;
            //client = new HttpClient(handler);
            //var uri = new Uri(Url);
            //client.BaseAddress = new Uri(Url);
            WdmsData.WdmsEntities wdmsdb = new WdmsData.WdmsEntities();
            string EmployeeDeviceCode = model.DeviceCode.ToString().PadLeft(9, '0');
            var employeeModel = (from c in wdmsdb.userinfo.Where(x => x.company_id == RiddhaSession.CompanyId && x.badgenumber == EmployeeDeviceCode)
                                 select new WDMSEmployeeUpdateModel()
                                 {
                                     pin = EmployeeDeviceCode,
                                     gender = c.Gender,
                                     idCard = c.Card,
                                     name = model.Name,
                                     password = c.Password,
                                     privilege = model.IsManager ? 6 : 0,
                                     departmentNumber = c.defaultdeptid ?? 0
                                 }
                                   ).FirstOrDefault();



            HttpResponseMessage response = client.PostAsJsonAsync<WDMSEmployeeUpdateModel>("/api/employees", employeeModel).Result;
            response.EnsureSuccessStatusCode();

            switch (response.StatusCode)
            {
                case HttpStatusCode.Accepted:
                    break;
                case HttpStatusCode.Ambiguous:
                    break;
                case HttpStatusCode.BadGateway:
                    break;
                case HttpStatusCode.BadRequest:
                    break;
                case HttpStatusCode.Conflict:
                    break;
                case HttpStatusCode.Continue:
                    break;
                case HttpStatusCode.Created:
                    break;
                case HttpStatusCode.ExpectationFailed:
                    break;
                case HttpStatusCode.Forbidden:
                    break;
                case HttpStatusCode.Found:
                    break;
                case HttpStatusCode.GatewayTimeout:
                    break;
                case HttpStatusCode.Gone:
                    break;
                case HttpStatusCode.HttpVersionNotSupported:
                    break;
                case HttpStatusCode.InternalServerError:
                    break;
                case HttpStatusCode.LengthRequired:
                    break;
                case HttpStatusCode.MethodNotAllowed:
                    break;
                case HttpStatusCode.Moved:
                    break;
                case HttpStatusCode.NoContent:
                    break;
                case HttpStatusCode.NonAuthoritativeInformation:
                    break;
                case HttpStatusCode.NotAcceptable:
                    break;
                case HttpStatusCode.NotFound:
                    break;
                case HttpStatusCode.NotImplemented:
                    break;
                case HttpStatusCode.NotModified:
                    break;
                case HttpStatusCode.OK:
                    return true;
                case HttpStatusCode.PartialContent:
                    break;
                case HttpStatusCode.PaymentRequired:
                    break;
                case HttpStatusCode.PreconditionFailed:
                    break;
                case HttpStatusCode.ProxyAuthenticationRequired:
                    break;
                case HttpStatusCode.RedirectKeepVerb:
                    break;
                case HttpStatusCode.RedirectMethod:
                    break;
                case HttpStatusCode.RequestEntityTooLarge:
                    break;
                case HttpStatusCode.RequestTimeout:
                    break;
                case HttpStatusCode.RequestUriTooLong:
                    break;
                case HttpStatusCode.RequestedRangeNotSatisfiable:
                    break;
                case HttpStatusCode.ResetContent:
                    break;

                case HttpStatusCode.ServiceUnavailable:
                    break;
                case HttpStatusCode.SwitchingProtocols:
                    break;

                case HttpStatusCode.Unauthorized:
                    break;
                case HttpStatusCode.UnsupportedMediaType:
                    break;
                case HttpStatusCode.Unused:
                    break;
                case HttpStatusCode.UpgradeRequired:
                    break;
                case HttpStatusCode.UseProxy:
                    break;
                default:
                    break;
            }

            return false;

        }
    }
    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
    }
    public class ApiResultBase
    {
        public string message { get; set; }
        public int code { get; set; }

    }
    public class WDMSEmployeeUpdateModel
    {
        public string pin { get; set; }
        public int departmentNumber { get; set; }
        public string name { get; set; }
        public string password { get; set; }
        public string idCard { get; set; }
        public int privilege { get; set; }
        public string gender { get; set; }
    }

}