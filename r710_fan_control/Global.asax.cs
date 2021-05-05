using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using r710_fan_control.Controllers;

namespace r710_fan_control
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static State State { get; set; }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
