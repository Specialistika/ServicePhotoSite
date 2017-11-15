using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Constructors.PathString;
using PhotoConsole.Domain.Data;
using PhotoConsole.Domain.Concrete;

namespace Constructors.ProductsSup
{
    public static class UploadProductsSup     
    {
        public static int UploadProducts()
        {
            int countUploadProduct = 0;
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
                            countUploadProduct++;
                            var inbarcode = new InBarcode
                            {
                                Barcode = Barcod[0].ToString(),
                                Code = Barcod[1].ToString(),
                                DateUpdate = DateTime.Now
                            };
                            db.InBarcode.Add(inbarcode);
                            db.SaveChanges();
                        };
                       
                    };
                };
            };
            }
            else 
            {
                throw new ArgumentNullException("File not found" );
            }
            return countUploadProduct;
        }
    }
}