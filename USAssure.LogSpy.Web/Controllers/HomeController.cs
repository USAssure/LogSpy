using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using USAssure.LogSpy.Data.Entities;
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
        public async Task<ActionResult> FindLogs(string environment, string appName, string query, string machineName, int hours)
        {
            if (string.IsNullOrEmpty(environment))
                environment = LogStores.Keys.First();

            var logs = await LogStores[environment.ToLowerInvariant()].FindLogs(query, hours <= 0 ? 24 : hours);

            if (string.IsNullOrEmpty(appName))
                appName = "All";     
            
            var model = new LogTableViewModel
            {
                Apps = GetAppList(logs, appName),
                Logs = 
                    !appName.Equals("All", System.StringComparison.InvariantCultureIgnoreCase) ?
                        logs.Where(l => l.AppName.Equals(appName, System.StringComparison.InvariantCultureIgnoreCase)).Select(ViewModelAdapter.ToLogListItemViewModel) : 
                        logs.Select(ViewModelAdapter.ToLogListItemViewModel)
            };

            return PartialView("LogTable", model);
        }

        [HttpGet]
        public ActionResult GetConfiguredEnvironments()
        {
            return PartialView("EnvironmentSelector", LogStores.Keys.Select(k => k));
        }

        private IEnumerable<AppViewModel> GetAppList(IEnumerable<Log> logs, string selectedApp)
        {
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

            if (!string.IsNullOrEmpty(selectedApp) && !selectedApp.Equals("all", System.StringComparison.InvariantCultureIgnoreCase))
                apps.First(a => a.Name.Equals(selectedApp, System.StringComparison.InvariantCultureIgnoreCase)).Selected = true;
            else
                apps.First(a => a.Name.Equals("All")).Selected = true;

            return apps;
        }
    }
}