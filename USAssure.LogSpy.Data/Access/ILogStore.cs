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
        Task<IEnumerable<Log>> GetAllLogs();
        Task<Log> GetLog(long id);
        Task<IEnumerable<Log>> GetLogs(string appName, string machineName);
        Task<IEnumerable<Log>> FindLogs(string appName, string query);
        Task<IEnumerable<App>> GetApps();
        Task<IEnumerable<string>> GetMachines();
    }
}
