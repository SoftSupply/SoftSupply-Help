using SoftSupply.Help.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SoftSupply.Help.Controllers
{
    public class DefaultController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Search(string area, string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {

                var contents = new System.IO.DirectoryInfo(Server.MapPath($"Areas\\{area}"));
                var files = contents.GetFiles("*.cshtml", System.IO.SearchOption.AllDirectories);

                var result = (from item in files
                              let body = System.IO.File.ReadAllText(item.FullName)
                              where body.Contains(searchString)
                              select item).FirstOrDefault();

                if (result != null)
                    return Redirect($"{area}?id={result.Directory.Name}&key={System.IO.Path.GetFileNameWithoutExtension(result.Name)}");

            }

            return Redirect(Request.UrlReferrer.ToString());
        }

    }
}