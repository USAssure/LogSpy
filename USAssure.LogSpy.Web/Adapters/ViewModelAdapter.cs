using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using USAssure.LogSpy.Data.Entities;
using USAssure.LogSpy.Web.Models;

namespace USAssure.LogSpy.Web.Adapters
{
    public static class ViewModelAdapter
    {
        public static LogViewModel ToLogViewModel(Log log)
        {
            return new LogViewModel
            {
                Id = log.Id,
                AppName = log.AppName,
                MachineName = log.MachineName,
                RecordedDate = log.RecordedDate,
                Level = log.Level,
                Type = log.Type,
                IpAddress = log.IpAddress,
                Host = log.Host,
                Url = log.Url,
                UserName = log.UserName,
                HttpMethod = log.HttpMethod,
                Message = log.Message,
                Exception = log.Exception,
                Payload = log.Payload,
                Keep = log.Keep,
                KeepUser = log.KeepUser
            };
        }

        public static LogListItemViewModel ToLogListItemViewModel(Log log)
        {
            return new LogListItemViewModel
            {
                Id = log.Id,
                RecordedDate = log.RecordedDate,
                AppName = log.AppName,
                MachineName = log.MachineName,
                Level = log.Level,
                Type = log.Type,
                Message = log.Message,
                Exception = log.Exception,
                Keep = log.Keep,
                KeepUser = log.KeepUser
            };
        }
    }
}