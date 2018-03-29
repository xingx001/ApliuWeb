using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class ClansController : Controller
    {
        //
        // GET: /Clans/

        public ActionResult Nb102zz()
        {
            return View();
        }

        //
        // GET: /Clans/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Clans/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Clans/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Clans/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Clans/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Clans/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Clans/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
