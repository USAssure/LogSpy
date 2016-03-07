using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using USAssure.LogSpy.Data.Entities;

namespace USAssure.LogSpy.Seed
{
    class Program
    {
        static void Main(string[] args)
        {
            var rng = new Random();
            var apps = new List<string> {"Registation", "Event Check In", "WorkplacePT", "usassure.com"};
            var machines = new List<string> {"ng-iis01a", "ng-sqlzuricv01", "ng-wss01a", "com01-sql2012"};
            var levels = new List<string> {"ERROR", "DEBUG", "INFO", "WARN"};
            var types = new List<string> {"exception", "sql", "web", "trace"};
            var methods = new List<string> {"get", "put", "post", "delete", "patch"};

            var list = new List<Log>();
            for (var x = 0; x < int.Parse(ConfigurationManager.AppSettings["Max"]); x++)
            {
                list.Add(new Log
                {
                    AppName = apps[rng.Next(0, apps.Count-1)],
                    MachineName = machines[rng.Next(0, machines.Count-1)],
                    RecordedDate = new DateTime(2016, rng.Next(1, 3), rng.Next(1, 28), rng.Next(1, 12), rng.Next(0, 59), rng.Next(0, 59)),
                    Level = levels[rng.Next(0, levels.Count-1)],
                    Type = types[rng.Next(0, types.Count-1)],
                    IpAddress = GenerateDummyIpV4Address(rng),
                    Host = new[] { "foo", "bar", "baz"}[rng.Next(0, 3)],
                    Url = null,
                    UserName = new[] { "susan", "sarah", "jimmy", "ted", "doris", "alfred" }[rng.Next(0, 6)],
                    HttpMethod = methods[rng.Next(0,methods.Count-1)],
                    Message = new[] { "Object not set to an instance of an object.", "Divide by zero.", "Just a trace log.", "Foo" }[rng.Next(0, 4)]
                });
            }

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Db"].ConnectionString))
            {
                const string query =
                    "insert into [LogSpy].[dbo].[Log] ([AppName], [MachineName], [RecordedDate], [Level], [Type], [IpAddress], [Host], [Url], [UserName], [HttpMethod], [Message]) values (@AppName, @MachineName, @RecordedDate, @Level, @Type, @IpAddress, @Host, @Url, @UserName, @HttpMethod, @Message)";
                connection.Execute(query, list);
            }
        }

        private static string GenerateDummyIpV4Address(Random rng)
        {
            return string.Format("{0}.{1}.{2}.{3}", rng.Next(0, 255), rng.Next(0, 255), rng.Next(0, 255),
                rng.Next(0, 255));
        }
    }
}
