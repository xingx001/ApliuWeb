using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class TextController : Controller
    {
        public ActionResult Index(string id)
        {
            return View((object)id);
        }
    }
}
