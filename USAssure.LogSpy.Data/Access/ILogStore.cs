using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USAssure.LogSpy.Data.Entities;

namespace USAssure.LogSpy.Data.Access
{
    public interface ILogStore
    {
        IEnumerable<Log> GetAllLogs();
        Log GetLog(long id);
        IEnumerable<Log> GetLogs(string appName, string machineName);
        IEnumerable<Log> FindLogs(string appName, string query);
        IEnumerable<App> GetApps();
        IEnumerable<string> GetMachines();
    }
}
