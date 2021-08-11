using System.Web.Mvc;

namespace RTech.Demo.Areas.HRM
{
    public class HRMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "HRM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "HRM_default",
                "HRM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}