using RTech.Demo.Filters;
using System.Web;
using System.Web.Mvc;

namespace RTech.Demo
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new RiddhaMVCAuthorizeAttribute());
            filters.Add(new MVCCompressionAttribute());
            System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new RiddhaAPIAuthorizeAttribute());
            System.Web.Http.GlobalConfiguration.Configuration.Filters.Add(new DeflateCompressionAttribute());
        }
    }
}
