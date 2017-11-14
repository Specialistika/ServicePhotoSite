using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Linq;
using System.Web.Http;
using TestFileUpload.module;
using System.Web.Mvc;
using Models.ForWeb;

namespace TestFileUpload
{
    public class FileUploadController : ApiController
    {
        [MimeMultipart]
        public async Task<FileUploadResult> Post()
        {
            var uploadPath = HttpContext.Current.Server.MapPath("~/img");

            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            string _localFileName = multipartFormDataStreamProvider
                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();

            return new FileUploadResult
            {
                LocalFilePath = _localFileName,

                FileName = Path.GetFileName(_localFileName),

                FileLength = new FileInfo(_localFileName).Length
            };
        }
    }
}