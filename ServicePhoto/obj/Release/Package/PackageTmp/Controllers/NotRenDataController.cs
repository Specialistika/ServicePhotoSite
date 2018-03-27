using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constructors.GridAction;
using Constructors.Extract;
using ExtractArtCode.Constructors;
using ExtractShtrih.Constructors;
using Constructors.OutInfoGrid;
using Constructors.NotRenameFiles;

namespace Controllers.NotRenameData
{
    public class NotRenDataController : Controller
    {
        // GET: NotRenData
        public ActionResult ViewNotRenData()
        {
            return View();
        }
        public JsonResult DontRenFilesModel(int? page, int? limit, string sortBy, string direction)
        {
            int total;
            var records = new NotRenFiles().WithoutRenFilesData(page, limit, sortBy, direction, out total);
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

    }
}