using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class GameController : Controller
    {
        //
        // GET: /Game/

        public ActionResult Pins()
        {
            return View();
        }
    }
}
