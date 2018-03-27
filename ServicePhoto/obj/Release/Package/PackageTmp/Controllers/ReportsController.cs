using System;
using System.Linq;
using System.Web.Mvc;
using UnZipFileForWeb.Models.DataClasses;

namespace UnZipFileForWeb.Controllers
{
    public class ReportsController : Controller
    {
        DataClasses objData;

        public ReportsController()
        {
            objData = new DataClasses();
        }

        //
        // GET: /Reports/

        public ActionResult Index()
        {
            var files = objData.GetFiles();
            return View(files);
        }


        public FileResult Download(string id)
        {
            int fid = Convert.ToInt32(id);
            var files = objData.GetFiles();
            string filename = (from f in files
                               where f.FileId == fid
                               select f.FilePath).First();
            string contentType = "application/csv";
            //Parameters to file are
            //1. The File Path on the File Server
            //2. The connent type MIME type
            //3. The paraneter for the file save asked by the browser
            return File(filename, contentType, "Report.csv");
        }

    }
}