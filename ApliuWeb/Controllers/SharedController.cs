﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ApliuWeb.Controllers
{
    public class SharedController : Controller
    {
        //
        // GET: /Shared/

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Templet()
        {
            return View();
        }

        //
        // GET: /Shared/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Shared/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Shared/Create

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
        // GET: /Shared/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Shared/Edit/5

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
        // GET: /Shared/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Shared/Delete/5

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
