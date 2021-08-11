using System.Web.Mvc;

namespace RTech.Demo.Areas.Office
{
    public class OfficeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Office";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Office_default",
                "Office/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}