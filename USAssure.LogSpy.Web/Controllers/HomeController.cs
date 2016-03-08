using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<ActionResult> GetLog(long id, string environment)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            var log = await LogStores[environment.ToLowerInvariant()].GetLog(id);
            return PartialView("ViewLog", ViewModelAdapter.ToLogViewModel(log));
        }

        [HttpGet]
        public async Task<ActionResult> GetLogs(string environment, string appName, string machineName)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            var logs = await LogStores[environment.ToLowerInvariant()].GetAllLogs();
            return PartialView("LogTable", logs.Select(ViewModelAdapter.ToLogListItemViewModel));
        }

        [HttpGet]
        public async Task<ActionResult> FindLogs(string environment, string appName, string query, string machineName)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            var logs = await LogStores[environment.ToLowerInvariant()].FindLogs(appName, query);
            return PartialView("LogTable", logs.Select(ViewModelAdapter.ToLogListItemViewModel));
        }

        [HttpGet]
        public async Task<ActionResult> GetLogCount(string environment)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            var logs = await LogStores[environment.ToLowerInvariant()].GetAllLogs();

            return Json(logs.Count(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetConfiguredEnvironments()
        {
            return PartialView("EnvironmentSelector", LogStores.Keys.Select(k => k));
        }

        [HttpGet]
        public async Task<ActionResult> GetApps(string environment)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            var logs = await LogStores[environment.ToLowerInvariant()].GetAllLogs();

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