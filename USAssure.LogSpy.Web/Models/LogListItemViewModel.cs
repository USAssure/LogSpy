using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USAssure.LogSpy.Web.Models
{
    public class LogListItemViewModel
    {
        public long Id { get; set; }
        public string AppName { get; set; }
        public string MachineName { get; set; }
        public DateTime RecordedDate { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public bool Keep { get; set; }
        public string KeepUser { get; set; }

        public string LogLevelStyle
        {
            get
            {
                switch(Level.ToLowerInvariant())
                {
                    case "debug":
                    case "info":
                        return "label-info";
                    case "warn":
                        return "label-warning";
                    case "error":
                    case "fatal":
                        return "label-danger";
                    default:
                        return "label-default";
                }
            }
        }
    }
}