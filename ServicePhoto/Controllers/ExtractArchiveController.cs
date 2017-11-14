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

namespace Controllers.Extract
{
    public class ExtractArchiveController : Controller
    {
        // GET: ExtractArchive
        public ActionResult ViewExtractArchive()
        {
            return View();
        }
        public JsonResult Search(string player = null)
        {
            DateTime dt;
            List<string> EstractArchive;
            ExtractArchive record = new ExtractArchive();

            if (DateTime.TryParse(player.ToString(), out dt))
            {
                record.extractArchive(dt, player = null, out EstractArchive );
            }
            else
            {
                record.extractArchive(dt = Convert.ToDateTime(null), player, out EstractArchive);
            }
            return Json(new { EstractArchive }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult OutInfosModel(int? page, int? limit, string sortB, string direction)
        {
            int total;
            var records = new GridModelOutInfo().OutInfos(page, limit, sortB, direction, out total );
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExtractArtMethod()
        {
            int count;
            string ExceptionMessage;
            var ExtrArt = new ExtractArt();
            ExtrArt.UpLoadArtCode(out count, out ExceptionMessage);
            return Json(new { count, ExceptionMessage }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ExtractShtrihMethod()
        {
            string ExtractArc;
            string UploadMainDir;
            var ExtrShtr = new ExtractShtrihCode();
            ExtractArc =  ExtrShtr.IntoBaseArchive().Count.ToString();
            UploadMainDir = ExtrShtr.RenFales().ToString();
            return Json(new { ExtractArc, UploadMainDir }, JsonRequestBehavior.AllowGet);
        }

    }

}