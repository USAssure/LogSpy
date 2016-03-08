using Dapper;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using USAssure.LogSpy.Data.Entities;
using System.Threading.Tasks;
using System.Data;

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

        public async Task<IEnumerable<Log>> FindLogs(string appName, string query)
        {
            if (string.IsNullOrEmpty(query))
                return await GetLogs(appName, null);

            return await WithConnection(async connection =>
            {
                var logs = await connection.QueryAsync<Log>("select * from [LogSpy].[dbo].[Log] where [IpAddress] like @query OR [Url] like @query OR [Message] like @query OR [Exception] like @query", new { query = string.Format("%{0}%", query) });
                return !string.IsNullOrEmpty(appName) && !appName.Equals("all", StringComparison.InvariantCultureIgnoreCase) ? logs.Where(l => l.AppName == appName) : logs;
            });
        }

        public async Task<IEnumerable<App>> GetApps()
        {
            return await WithConnection(async connection =>
            {
                return await connection.QueryAsync<App>("select [AppName], count(*) as 'LogCount' from [LogSpy].[dbo].[Log] group by [AppName] order by [AppName] asc");
            });
        }

        public async Task<IEnumerable<Log>> GetAllLogs()
        {
            return await WithConnection(async connection =>
            {
                return await connection.QueryAsync<Log>("select * from [LogSpy].[dbo].[Log] order by [RecordedDate] desc");
            });
        }

        public async Task<Log> GetLog(long id)
        {
            return await WithConnection(async connection =>
            {
                return (await connection.QueryAsync<Log>("select top 1 * from [LogSpy].[dbo].[Log] where [Id] = @Id", new { id = id })).Single();
            });
        }

        public async Task<IEnumerable<Log>> GetLogs(string appName, string machineName)
        {
            return await WithConnection(async connection =>
            {
                var logs = await connection.QueryAsync<Log>("select * from [LogSpy].[dbo].[Log] order by [RecordedDate] desc");
                if (!string.IsNullOrEmpty(appName) && !appName.Equals("all", StringComparison.InvariantCultureIgnoreCase))
                    logs = logs.Where(a => a.AppName == appName);

                if (!string.IsNullOrEmpty(machineName))
                    logs = logs.Where(m => m.MachineName == machineName);

                return logs;
            });
        }

        public async Task<IEnumerable<string>> GetMachines()
        {
            return await WithConnection(async connection =>
            {
                return await connection.QueryAsync<string>("select distinct [MachineName] from [LogSpy].[dbo].[Log] order by [MachineName] asc");
            });
        }

        private async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await getData(connection);
            }
        }
    }
}
