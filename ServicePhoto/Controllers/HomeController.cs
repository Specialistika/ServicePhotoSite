using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constructors.ProductsSup;

namespace Controllers.Home
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
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
        public ActionResult UpdateProduct()
        {
            return View();
        }
        public void UploadProductMuthod()
        {
            int countProd = UploadProductsSup.startUploadMethod();
        }
        public void StopUploadProduct()
        {
            UploadProductsSup.stopUploadMethod();
        }
        public JsonResult countProcess()
        {
            string threadStatus;
            string exeptionAttrstop;
            UploadProductsSup.atributeStop(out threadStatus, out exeptionAttrstop);
            string countRecords = UploadProductsSup.countRecords();
            return Json(new { countRecords, threadStatus, exeptionAttrstop }, JsonRequestBehavior.AllowGet);
        }
    }
}