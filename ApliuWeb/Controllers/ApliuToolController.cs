using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class ApliuToolController : Controller
    {
        //
        // GET: /ApliuTool/

        public ActionResult QRCode()
        {
            return View();
        }

        //
        // GET: /ApliuTool/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ApliuTool/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ApliuTool/Create

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
        // GET: /ApliuTool/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ApliuTool/Edit/5

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
        // GET: /ApliuTool/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ApliuTool/Delete/5

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
