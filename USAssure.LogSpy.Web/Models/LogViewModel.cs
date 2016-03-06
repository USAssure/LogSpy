using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USAssure.LogSpy.Web.Models
{
    public class LogViewModel
    {
        public long Id { get; set; }
        public string AppName { get; set; }
        public string MachineName { get; set; }
        public DateTime RecordedDate { get; set; }
        public string Level { get; set; }
        public string Type { get; set; }
        public string IpAddress { get; set; }
        public string Host { get; set; }
        public string Url { get; set; }
        public string UserName { get; set; }
        public string HttpMethod { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Payload { get; set; }
        public bool Keep { get; set; }
        public string KeepUser { get; set; }
    }
}