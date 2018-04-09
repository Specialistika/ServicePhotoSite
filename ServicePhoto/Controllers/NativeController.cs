using System;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using Constructors.PathString;
using Constructor.PartFileNative;
using System.Text;
using System.Net;
using PhotoConsole.Domain.Data;
using System.Web;

namespace Controllers.Native
{
    public class NativeController : Controller
    {
        //private int ammount = 0;
        public ActionResult UploadFileView()
        {
            return View();
        }
        [HttpGet]
        public JsonResult UploadDir(string DirChunk)
        {
            string pathDirectory = null;
            switch (DirChunk)
            {
                case "NativeDir":
                    pathDirectory = VariableConfig.NativePath;
                    break;
                case "ForWebDir":
                    pathDirectory = VariableConfig.ForWebPath;
                    break;
                case "MobileDir":
                    pathDirectory = VariableConfig.for_mobilePath;
                    break;
            }
            return Json(new { pathDirectory }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpResponseMessage UploadFileChuck()
        {
            //int FileCount =0;
            //int FileIndex = 0;
            //string partToken = ".part_";
            string path = null;

            foreach (string file in Request.Files)
            {
                var FileDataContent = Request.Files[file];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    path = Path.Combine(Request.Form.Keys[0], fileName);
                    //string trailingTokens = path.Substring(path.IndexOf(partToken) + partToken.Length);
                    //int.TryParse(trailingTokens.Substring(trailingTokens.IndexOf(".") + 1), out FileCount);
                    //int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out FileIndex);
                    try
                    {
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        PartNative UT = new PartNative();
                        UT.MergeFileNative(path);
                    }
                    catch (IOException ex)
                    {
                        string errorChank = ("Блок заблокирован" + ex.Message).Substring(0,80);
                        using (var db = new RenFilesEntities())
                        {
                            var Error = new ErrorFix { Error = errorChank, DateInsert = DateTime.Now };
                            db.ErrorFix.Add(Error);
                            db.SaveChanges();
                        }
                    }
                }
            };
            HttpResponseMessage httpMessage = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent("File uploaded.")
            };
            return httpMessage;
        }
  
    }
}