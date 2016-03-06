using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using USAssure.LogSpy.Data.Access;
using USAssure.LogSpy.Web.Models;

namespace USAssure.LogSpy.Web.Controllers
{
    public abstract class BaseController : Controller
    {
        private IDictionary<string, ILogStore> _logStores;

        protected IDictionary<string, ILogStore> LogStores
        {
            get
            {
                if (_logStores != null && _logStores.Any())
                    return _logStores;

                _logStores = new Dictionary<string, ILogStore>();

                var environments = HttpContext.Application["environments"] as EnvironmentConfigModel;
                foreach(var environment in environments.Environments)
                {
                    _logStores.Add(environment.Name.ToLowerInvariant(), new LogStore(environment.ConnectionString));
                }

                return _logStores;
            }
        }
    }
}