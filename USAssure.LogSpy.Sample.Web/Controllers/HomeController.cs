using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace USAssure.LogSpy.Sample.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {
            _logger.Error("test");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}