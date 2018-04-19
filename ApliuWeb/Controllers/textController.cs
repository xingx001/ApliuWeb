using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class textController : Controller
    {
        public ActionResult Index(int? id)
        {
            return View(id);
        }
    }
}
