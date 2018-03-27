using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoConsole.Domain.Entities
{
    public class IntoCap
	{
        public int Id { get; set; }
        public string ToCapture { get; set; }
        public DateTime? Dateloginto { get; set; }
    }
    public class Categories
    {
        public int id { get; set; }
        public string Category { get; set; }
    }
    public class OutInfos
    {
        public int Id { get; set; }
        public string Barlog { get; set; }
        public string Codelog { get; set; }
        public Nullable<System.DateTime> Datelog { get; set; }
        public Nullable<bool> Renlog { get; set; }
    }
    public class ForWebTests
    { 
    public int Id { get; set; }
    public string InfoDate { get; set; }
    public Nullable<System.DateTime> Data { get; set; }
    }
    public class InCaptures
    {
        public int Id { get; set; }
        public string Capture { get; set; }
        public Nullable<System.DateTime> Dateinto { get; set; }
		public string FolderName { get; set; }
    }
    public class OutInCapture
    {
        public int Id { get; set; }
        public string Capture { get; set; }
        public Nullable<System.DateTime> Dateinto { get; set; }
    }
    public class Not_inBases
    {
        public int Idd { get; set; }
        public string NotCapture { get; set; }
        public Nullable<System.DateTime> DatelogNotData { get; set; }
    }
    public class InBarcodes
    {
        public int Id { get; set; }
        public string Barcode { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> DateUpdate { get; set; }

    }
	public class ErrorFixes
	{
		public int Id { get; set; }
		public string Error { get; set; }
		public Nullable<System.DateTime> DateInsert { get; set; }
	}

}
