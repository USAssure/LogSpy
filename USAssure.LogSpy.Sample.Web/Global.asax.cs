using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using USAssure.LogSpy.Sample.Web.Utility;

namespace USAssure.LogSpy.Sample.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //log4net
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/" + ConfigurationManager.AppSettings["loggingConfig"])));
            log4net.GlobalContext.Properties["appName"] = ConfigurationManager.AppSettings["AppName"];
            log4net.GlobalContext.Properties["userName"] = HttpContextValueProvider.HttpUserName;
            log4net.GlobalContext.Properties["ipAddress"] = HttpContextValueProvider.HttpIpAddress;
            log4net.GlobalContext.Properties["url"] = HttpContextValueProvider.HttpUrl;
            log4net.Util.SystemInfo.NullText = null;
        }
    }
}
