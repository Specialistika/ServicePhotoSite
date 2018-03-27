using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Constructors.PathString;
using Models.ForWeb;
using PhotoConsole.Domain.Data;
using PhotoConsole.Domain.Entities;

namespace ExtractArtCode.Constructors
{
    public class ExtractArt
    {
        public void UpLoadArtCode(out int count, out string ExceptionMessage)
        {
            char[] delimiterChars = { ';' };

            string[] files = Directory.GetFiles(VariableConfig.ShtrihcodPath); //capture

            var dir = new DirectoryInfo(VariableConfig.ShtrihcodPath); //capture

            var filess = new List<string>();

            ExceptionMessage = "";

            foreach (FileInfo file in dir.GetFiles()) //capture
            {
                filess.Add(Path.GetFileNameWithoutExtension(file.FullName));
            }
            using (var db = new RenFilesEntities())
            for (int m = 0; m < files.Length; m++)
            {
                    try
                    {
                        File.Copy(files[m], VariableConfig.ImagesPath + filess[m] + ".png", true);
                        File.SetCreationTime(files[m], DateTime.Now);
                        File.SetLastWriteTime(files[m], DateTime.Now);
                        var Update = new OutInfo
                        {
                            Barlog = Convert.ToString("Null"),
                            Codelog = Convert.ToString(filess[m]),
                            Datelog = DateTime.Now,
                            Renlog = Convert.ToBoolean(true)
                        };
                        db.OutInfo.Add(Update);
                    }
                    catch (IOException s)
                    {
                        ExceptionMessage = s.Message;
                    }
                 db.SaveChanges();
            }
            foreach (string del in files)
            {
                File.Delete(del);
            }
            if (files.Count() != 0)
            {
                count = files.Count();
            }
            else
            {
                count = 0;
            }

        }
    }
}
