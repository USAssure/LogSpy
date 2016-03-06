using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace USAssure.LogSpy.Sample.Web.Utility
{
    public static class HttpContextValueProvider
    {
        public static HttpContextUserNameProvider HttpUserName
        {
            get
            {
                return new HttpContextUserNameProvider();
            }
        }

        public static HttpContextIpAddressProvider HttpIpAddress
        {
            get
            {
                return new HttpContextIpAddressProvider();
            }
        }

        public static HttpContextUrlProvider HttpUrl
        {
            get
            {
                return new HttpContextUrlProvider();
            }
        }
    }

    public abstract class BaseHttpContextProvider : IFixingRequired
    {
        protected HttpContext _context = HttpContext.Current;

        public abstract override string ToString();

        object IFixingRequired.GetFixedObject()
        {
            return ToString();
        }
    }

    public sealed class HttpContextUserNameProvider : BaseHttpContextProvider
    {
        public override string ToString()
        {
            if (_context != null && _context.User != null && _context.User.Identity.IsAuthenticated)
            {
                return _context.User.Identity.Name;
            }

            return null;
        }
    }

    public sealed class HttpContextIpAddressProvider : BaseHttpContextProvider
    {
        public override string ToString()
        {
            if(_context.Request != null)
            {
                var proxyIp = _context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(proxyIp))
                    return proxyIp.Split(',').Last();

                return _context.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }

            return null;
        }
    }

    public sealed class HttpContextUrlProvider : BaseHttpContextProvider
    {
        public override string ToString()
        {
            return _context.Request != null ? _context.Request.Path : null;
        }
    }
}