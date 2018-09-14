using System.Web.Mvc;

namespace FytMsys.Logic.Admin
{
    public class FytAdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FytAdmin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FytAdmin_default",
                "FytAdmin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new string[1] { "FytMsys.Logic.Admin" }
            );
        }
    }
}