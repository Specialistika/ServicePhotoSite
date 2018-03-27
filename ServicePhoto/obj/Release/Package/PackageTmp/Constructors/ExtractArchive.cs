using Constructors.PathString;
using Constructors.UnzipModule;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Constructors.Extract
{
    public class ExtractArchive
    {
        public void extractArchive(DateTime dt, string st, out List<string> EstractArchive)
        {
            FileInfo[] files = null;

            DirectoryInfo info = new DirectoryInfo(VariableConfig.ForWebPath);

            if (dt != Convert.ToDateTime(null))
            {
                files = info.GetFiles("*.zip").Where(p => p.CreationTime > dt).ToArray();
            }
            else if (dt == Convert.ToDateTime(null))
            {
                files = info.GetFiles("*.zip").Where(p => p.Name == st).ToArray();
            }

            for (int m = 0; m < files.Length; m++)
            {
                try
                {
                    File.Copy(VariableConfig.ForWebPath + files[m], Path.Combine(VariableConfig.ShtrihcodPath + files[m]), true);

                    using (var unzip = new Unzip(VariableConfig.ShtrihcodPath + files[m]))
                    {
                        unzip.ExtractToDirectory(VariableConfig.ShtrihcodPath);
                    }
                }
                catch (IOException d)
                {
                    Console.WriteLine(d.Message);
                }
                finally
                {
                    File.Delete(VariableConfig.ShtrihcodPath + files[m]);
                }

            }
            EstractArchive = null; 
            foreach (var names in files)
            {
                EstractArchive = new List<string>(new string[] { names.ToString() });
            }
        }
    }
}