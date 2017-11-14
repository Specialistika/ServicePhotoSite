using System;
using System.IO;
using System.Net.Http;
using System.Web.Mvc;
using Constructors.PathString;
using Constructor.PartFileNative;

namespace Controllers.Native
{
    public class NativeController : Controller
    {
        public ActionResult UploadNativeView()
        {
            return View();
        }

        // generic file post method - use in MVC or WebAPI
        [HttpPost]
        public HttpResponseMessage UploadFileNative()
        {
            foreach (string file in Request.Files)
            {
                var FileDataContent = Request.Files[file];
                if (FileDataContent != null && FileDataContent.ContentLength > 0)
                {
                    // take the input stream, and save it to a temp folder using the original file.part name posted
                    var stream = FileDataContent.InputStream;
                    var fileName = Path.GetFileName(FileDataContent.FileName);
                    string path = Path.Combine(VariableConfig.NativePath, fileName);
                    try
                    {
                        if (System.IO.File.Exists(path))
                            System.IO.File.Delete(path);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        PartNative UT = new PartNative();
                        UT.MergeFileNative(path);
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