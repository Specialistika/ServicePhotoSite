using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Constructors.PathString;
using PhotoConsole.Domain.Data;
using PhotoConsole.Domain.Concrete;
using System.Threading.Tasks;
using System.Threading;



namespace Constructors.ProductsSup
{
    public static class UploadProductsSup
    {
        static Thread action = null;
        private static void UploadProducts()
        {
            var dbdata = new EFProductRepository();
            char[] delimiterChars = { ';' };
            string[] Barcod = null;
            string fileName = VariableConfig.ReportPath + VariableConfig.fileProductSup;
            FileInfo fullFileName = new FileInfo(fileName);

            if (fullFileName.Exists)
            {
                DirectoryInfo dirFile = fullFileName.Directory;

                string[] Barcods = File.ReadAllLines(fileName);
            
            using (var db = new RenFilesEntities1())
            {
                for (int i = 0; i < Barcods.Length; i++)
                {
                    if (!String.IsNullOrEmpty(Barcods[i]))
                    {
                        Barcod = Barcods[i].Split(delimiterChars);
                        
                        string Bar = Barcod[0].ToString();
         
                        var seekProduct = dbdata.InBarcodes.Where(c => c.Barcode == Bar).FirstOrDefault();

                        if (seekProduct == null)
                        {
                            try
                            {
                                var inbarcode = new InBarcode
                                {
                                    Barcode = Barcod[0].ToString(),
                                    Code = Barcod[1].ToString(),
                                    DateUpdate = DateTime.Now
                                };
                                    db.InBarcode.Add(inbarcode);
                                    db.SaveChanges();
                            }
                            catch (InvalidOperationException e)
                            {
                                string showExeption = e.Message.ToString();
                            }
                        };
                       
                    };
                };
            };
            }
            else 
            {
                throw new ArgumentNullException("File not found" );
            }
        }
            public static int startUploadMethod()
            {
                int countt = 0;
                string exeptionAttrstop = null;
                try
                {
                    action = new Thread(() => { UploadProducts(); });
                    action.Start();
                }
                catch (ThreadInterruptedException e)
                {
                    exeptionAttrstop = "Поток прерван" + e.Message;
                }
                    return countt;
            }
            public static void stopUploadMethod()
            { 
                if (action != null)
                {
                    try 
                    {
                        action.Abort();
                        action.Join();
                    } 
                    catch(ThreadAbortException e) 
                    {
                       string exeption = e.Message;
                    }
                }
            }
            public static string countRecords()
            {
                int countRecords = 0;

                using (var db = new RenFilesEntities1())
                {
                    countRecords = db.InBarcode.Count(); 
                };
                    return countRecords.ToString();
            }
            public static void atributeStop(out string threadStatus, out string exeptionAttrstop)
            {
                threadStatus = null;
                exeptionAttrstop = null;

                try
                {
                    threadStatus = "" + action.ThreadState;
                }
                catch (ThreadInterruptedException e)
                {
                    exeptionAttrstop = "Поток прерван" + e.Message;
                }
            }
      }
}