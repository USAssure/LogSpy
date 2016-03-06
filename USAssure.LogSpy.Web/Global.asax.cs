using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using USAssure.LogSpy.Web.Models;

namespace USAssure.LogSpy.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            var json = File.ReadAllText(Server.MapPath("~/" + ConfigurationManager.AppSettings["env.json"]));
            var environments = JsonConvert.DeserializeObject<EnvironmentConfigModel>(json);
            if (environments == null)
                throw new FileNotFoundException("Environments config not found.");

            if(!environments.Environments.Any())
                throw new ArgumentException(string.Format("No environments defined in appSetting {0}.", ConfigurationManager.AppSettings["env.json"]));

            Application["environments"] = environments;
        }
    }
}
