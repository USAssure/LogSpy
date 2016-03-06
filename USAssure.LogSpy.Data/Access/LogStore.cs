using Dapper;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using USAssure.LogSpy.Data.Entities;

namespace USAssure.LogSpy.Data.Access
{
    public class LogStore : ILogStore
    {
        private readonly string _connectionString;

        public LogStore(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString");

            _connectionString = connectionString;
        }

        private SqlConnection _sqlConnection;

        private SqlConnection _connection => _sqlConnection ?? (_sqlConnection = new SqlConnection(_connectionString));

        public IEnumerable<Log> FindLogs(string appName, string query)
        {
            if (string.IsNullOrEmpty(query))
                return GetLogs(appName, null);

            using(var connection = _connection)
            {
                var logs = connection.Query<Log>("select * from [LogSpy].[dbo].[Log] where [IpAddress] like @query OR [Url] like @query OR [Message] like @query OR [Exception] like @query", new { query = string.Format("%{0}%", query) });
                return !string.IsNullOrEmpty(appName) && !appName.Equals("all", StringComparison.InvariantCultureIgnoreCase) ? logs.Where(l => l.AppName == appName) : logs;
            }
        }

        public IEnumerable<App> GetApps()
        {
            using (var connection = _connection)
            {
                return connection.Query<App>("select [AppName], count(*) as 'LogCount' from [LogSpy].[dbo].[Log] group by [AppName] order by [AppName] asc");
            }
        }

        public IEnumerable<Log> GetAllLogs()
        {
            using (var connection = _connection)
            {
                return connection.Query<Log>("select * from [LogSpy].[dbo].[Log] order by [RecordedDate] desc");
            }
        }

        public Log GetLog(long id)
        {
            using (var connection = _connection)
            {
                return connection.Query<Log>("select top 1 * from [LogSpy].[dbo].[Log] where [Id] = @id", new { id = id }).Single();
            }
        }

        public IEnumerable<Log> GetLogs(string appName, string machineName)
        {
            using (var connection = _connection)
            {
                var logs = connection.Query<Log>("select * from [LogSpy].[dbo].[Log] order by [RecordedDate] desc");
                if (!string.IsNullOrEmpty(appName) && !appName.Equals("all", StringComparison.InvariantCultureIgnoreCase))
                    logs = logs.Where(a => a.AppName == appName);

                if (!string.IsNullOrEmpty(machineName))
                    logs = logs.Where(m => m.MachineName == machineName);

                return logs;
            }
        }

        public IEnumerable<string> GetMachines()
        {
            using (var connection = _connection)
            {
                return connection.Query<string>("select distinct [MachineName] from [LogSpy].[dbo].[Log] order by [MachineName] asc");
            }
        }
    }
}
