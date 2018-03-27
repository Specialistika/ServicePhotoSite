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
        public static IEnumerable<OutInfos> OutInfos { get; set; }
        public static IEnumerable<ForWebTests> ForWebTests { get; set; }
        public static IEnumerable<InCaptures> InCaptures { get; set; }
        public static IEnumerable<Not_inBases> Not_inBase { get; set; }
        public static IEnumerable<OutInCapture> OutInCapture { get; set; }
        public static IEnumerable<InBarcodes> InBarcodes { get; set; }
		public static IEnumerable<ErrorFixes> ErrorFix { get; set; }
		public static Pageinfo pageinfo { get; set; }

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