using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using USAssure.LogSpy.Web.Adapters;
using USAssure.LogSpy.Web.Models;

namespace USAssure.LogSpy.Web.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLog(long id, string environment)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            var log = LogStores[environment.ToLowerInvariant()].GetLog(id);
            return PartialView("ViewLog", ViewModelAdapter.ToLogViewModel(log));
        }

        [HttpGet]
        public ActionResult GetLogs(string environment, string appName, string machineName)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            return PartialView("LogTable", 
                LogStores[environment.ToLowerInvariant()].GetAllLogs().Select(ViewModelAdapter.ToLogListItemViewModel));
        }

        [HttpGet]
        public ActionResult FindLogs(string environment, string appName, string query, string machineName)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            return PartialView("LogTable", 
                LogStores[environment.ToLowerInvariant()].FindLogs(appName, query).Select(ViewModelAdapter.ToLogListItemViewModel));
        }

        [HttpGet]
        public ActionResult GetLogCount(string environment)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            return Json(LogStores[environment.ToLowerInvariant()].GetAllLogs().Count(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetConfiguredEnvironments()
        {
            return PartialView("EnvironmentSelector", LogStores.Keys.Select(k => k));
        }

        [HttpGet]
        public ActionResult GetApps(string environment)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            var logs = LogStores[environment.ToLowerInvariant()].GetAllLogs();

            var apps = new List<AppViewModel>
            {
                new AppViewModel
                {
                    Name = "All",
                    LogCount = logs.Count()
                }
            };

            apps.AddRange(logs.GroupBy(a => a.AppName, (key, grp) => new { Name = key, LogCount = grp.Count() }).Select(a => new AppViewModel
            {
                Name = a.Name,
                LogCount = a.LogCount
            }));

            return PartialView("AppBar", apps);
        }
    }
}