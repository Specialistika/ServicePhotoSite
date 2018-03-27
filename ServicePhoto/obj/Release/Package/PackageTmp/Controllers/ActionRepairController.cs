using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Repair.context;

namespace UnZipFileForWeb.Controllers
{
    public class ActionRepairController : RapairController
    {
        public ActionResult View_Repair()
        {
            return View();
        }
    }
}