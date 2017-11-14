using System;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using Constructor.blockForWeb;
using Constructors.PathString;

namespace Controllers.UploadForWeb
{
    public class ForWebController : Controller
    {

        public ActionResult UploadForWebView()
        {
            return View();
        }

        [HttpPost]
        public HttpResponseMessage UploadForWebMethod()
        {
            foreach (string file in Request.Files)
            {
                var FileDataContent = Request.Files[file];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    string path = Path.Combine(VariableConfig.ForWebPath, fileName);
                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        var UT = new Part_blockForWeb();
                        UT.MergeFileForWeb(path);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
                return new HttpResponseMessage()
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("File uploaded.")
                };
        }
	}
}