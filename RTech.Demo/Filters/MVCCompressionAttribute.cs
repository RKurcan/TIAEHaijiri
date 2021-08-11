using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo.Filters
{
    public class MVCCompressionAttribute:ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext actContext)
        {
            HttpResponse Response = HttpContext.Current.Response;

            if (IsGZipSupported())
            {
                string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

                if (AcceptEncoding.Contains("gzip"))
                {
                    Response.Filter = new System.IO.Compression.GZipStream(Response.Filter,
                                                System.IO.Compression.CompressionMode.Compress);
                    Response.Headers.Remove("Content-Encoding");
                    try
                    {
                        Response.AppendHeader("Content-Encoding", "gzip");
                    }
                    catch (Exception)
                    {
                    }
                    
                }
                else
                {
                    Response.Filter = new System.IO.Compression.DeflateStream(Response.Filter,
                                                System.IO.Compression.CompressionMode.Compress);
                    Response.Headers.Remove("Content-Encoding");
                    Response.AppendHeader("Content-Encoding", "deflate");
                }
            }

            // Allow proxy servers to cache encoded and unencoded versions separately
            try
            {
                Response.AppendHeader("Vary", "Content-Encoding");
            base.OnActionExecuted(actContext);
            }
            catch (Exception)
            {
            }
        }
        public static bool IsGZipSupported()
        {
            string AcceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];
            if (!string.IsNullOrEmpty(AcceptEncoding) &&
                    (AcceptEncoding.Contains("gzip") || AcceptEncoding.Contains("deflate")))
                return true;
            return false;
        }
    }
}