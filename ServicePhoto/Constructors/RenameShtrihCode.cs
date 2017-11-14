using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.Entity;
using System.Data;
using Constructors.PathString;
using PhotoConsole.Domain.Data;

namespace ExtractShtrih.Constructors
{
    public class ExtractShtrihCode
    {
        private string[] files = Directory.GetFiles(VariableConfig.ShtrihcodPath); //capture

        private string[] Barcod = null;

        private char[] delimiterChars = { ';' };

        List<string> getList = new List<string>();

        //пополнение таблицы с данными из директории ForWeb (архивов) в таблицу inCapture
        public List<string> IntoBaseArchive()
        {
            var dir = new DirectoryInfo(VariableConfig.ShtrihcodPath); //capture

            var filess = new List<string>();

#region 
            //очищение таблицы с данными выложенными в дирекрорию ForWeb

            

            //using (var db = new Renamer.Data.BarcodeEntities())
            //{
            //    foreach (var u in db.InCaptures)
            //    {
            //        var itemsdeleted = db.InCaptures.Remove(u);
            //    }
            //    db.SaveChanges();
            //}
#endregion

            foreach (FileInfo file in dir.GetFiles()) //capture 
            {
                filess.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }
            using (var db = new RenFilesEntities1())
            {
                for (int h = 0; h < files.Length; h++)
                {
                    Barcod = filess[h].Split(delimiterChars);
                    OutInCapture InCap = new OutInCapture
                    {
                        Capture = Convert.ToString(Barcod[0]),
                        Dateinto = DateTime.Now
                    };
                    db.OutInCapture.Add(InCap);
                    db.SaveChanges();
                }
            }
            return getList = filess ;
        }

        //Если в базе Кристалла есть баркод, то переименовать и внести данные в таблицу OutInfo
        public Int32 RenFales()
        {
            int count_out = 0;

            using (var db = new RenFilesEntities1())
            {
                for (int m = 0; m < files.Length; m++)
                {
                    Barcod = getList[m].Split(delimiterChars);
                    var tmp = Barcod[0];
                    InBarcode brc = db.InBarcode.Where(c => c.Barcode == tmp).FirstOrDefault();

                    if (brc != null)
                    {
                        count_out++;
                        try
                        {
                            File.Copy(files[m], VariableConfig.ImagesPath + brc.Code + ".png", true);
                            File.SetCreationTime(files[m], DateTime.Now);
                            File.SetLastWriteTime(files[m], DateTime.Now);
                            OutInfo intest = new OutInfo
                            {
                                Barlog = Convert.ToString(brc.Barcode),
                                Codelog = Convert.ToString(brc.Code),
                                Datelog = DateTime.Now,
                                Renlog = Convert.ToBoolean(true)
                            };
                            db.OutInfo.Add(intest);
                        }
                        catch (IOException s)
                        {
                            if (s.Data != null)
                            {
                                OutInfo intests = new OutInfo
                                {
                                    Barlog = Convert.ToString(tmp),
                                    Codelog = Convert.ToString("double"),
                                    Datelog = DateTime.Now,
                                    Renlog = false
                                };
                                db.OutInfo.Add(intests);
                            }
                        }
                    }
                    else
                    {
                        Not_inBase mainDir = new Not_inBase
                        {
                            Capture = Convert.ToString(tmp),
                            Datelog = DateTime.Now
                        };
                        db.Not_inBase.Add(mainDir);
                    }
                    db.SaveChanges();
                }
            }
            foreach (string del in files)
            {
                File.Delete(del);
            }
            return count_out;
        }
     }
}
