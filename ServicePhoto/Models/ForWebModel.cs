using System;
using System.Collections.Generic;
using System.Web;
using PhotoConsole.Domain.Entities;
using UnZipFileForWeb.HtmlHelpers;
using ModelsPageinfo;

namespace Models.ForWeb
{
    public class ForWebModel
        //: IDisposable
    {
        //bool disposed = true;
        public IEnumerable<OutInfos> OutInfos { get; set; }
        public IEnumerable<ForWebTests> ForWebTests { get; set; }
        public IEnumerable<InCaptures> InCaptures { get; set; }
        public IEnumerable<Not_inBases> Not_inBase { get; set; }
        public IEnumerable<OutInCapture> OutInCapture { get; set; }
        public IEnumerable<InBarcodes> IntoCapture { get; set; }
        public Pageinfo pageinfo { get; set; }

        //public void Dispose(Boolean itIsSafeToAlsoFreeManagedObjects)
        //{
        //    //throw new NotImplementedException();
        //}
    }
    public class FileUploadResult
    {
        public string LocalFilePath { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
    }
}