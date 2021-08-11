using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Controllers
{
    public class IClockController : Controller
    {
        //
        // GET: /IClock/
        //[HttpGet]
        [HttpPost]
        public string CData(string SN)
        {
            Response.ContentType = "text/plain";
            string stamp = "";
            string opStamp = "";
            return "GET OPTION FROM:" + Request.QueryString["SN"]+
                    "Stamp=" + stamp +
                    "OpStamp=" + opStamp +
                    "ErrorDelay=60" +
                    "Delay=30" +
                    "ResLogDay=18250" +
                    "ResLogDelCount=10000" +
                    "ResLogCount=50000" +
                    "TransTimes=00:00;14:05" +
                    "TransInterval=1" +
                    "TransFlag=1111000000" +
                    "Realtime=1" +
                    "Encrypt=0";
        }
        public string GetRequest(string SN)
        {
            Response.ContentType = "text/plain";
            return "HTTP/1.1 200 OK" +
                    "Content-Type: text/plain" +
                    "Date: Thu, 19 Feb 2008 15:52:10 GMT";
        }
        public string DeviceCmd(string SN)
        {
            Response.ContentType = "text/plain";
            return "HTTP/1.1 200 OK" +
                    "Content-Type: text/plain" +
                    "Date: Thu, 19 Feb 2008 15:52:10 GMT";
        }
    }
}