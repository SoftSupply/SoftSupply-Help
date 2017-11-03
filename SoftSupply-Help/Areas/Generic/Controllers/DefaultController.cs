using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftSupply.Help.Areas.Generic.Content
{
    public class DefaultController : Controller
    {
        // GET: Generic/Default
        public ActionResult Index()
        {
            return View();
        }
    }
}