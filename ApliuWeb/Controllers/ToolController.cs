using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class ToolController : Controller
    {
        //
        // GET: /Tool/

        public ActionResult QRCode()
        {
            return View();
        }

        public ActionResult SqlServer()
        {
            return View();
        }

        public ActionResult SMSSend()
        {
            return View();
        }

        //
        // GET: /Tool/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Tool/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Tool/Create

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
        // GET: /Tool/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Tool/Edit/5

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
        // GET: /Tool/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Tool/Delete/5

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
