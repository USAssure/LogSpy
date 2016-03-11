using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USAssure.LogSpy.Web.Models
{
    public class LogTableViewModel
    {
        public IEnumerable<AppViewModel> Apps { get; set; }
        public IEnumerable<LogListItemViewModel> Logs { get; set; }

        public LogTableViewModel()
        {
            Apps = new List<AppViewModel>();
            Logs = new List<LogListItemViewModel>();
        }
    }
}