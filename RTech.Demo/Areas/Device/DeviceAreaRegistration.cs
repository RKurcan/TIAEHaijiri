using System.Web.Mvc;

namespace RTech.Demo.Areas.Device
{
    public class DeviceAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Device";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Device_default",
                "Device/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}