using System;
using System.IO;
using ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.Archiver;
using System.IO.Compression;
using PhotoConsole.Domain.Data;
using Constructors.PathString;
using Models.ForWeb;

namespace Conctructor.UnZipFileForWeb
{
    public class UnZipForWeb : ForWebModel
	{
        //public string SourceFile { get; set; }

        public static void Extract(string SourceFile)
        {
            using (var archiver = new ZipForge())
            {
                try
                {
                    archiver.FileName = SourceFile;
                    archiver.OpenArchive(System.IO.FileMode.Open);
                    archiver.BaseDir = VariableConfig.ForWebPath;
                    archiver.ExtractFiles("*.*");
                    archiver.CloseArchive();
                }
                catch (ArchiverException ae)
                {
                    Console.WriteLine("Message: {0}\t Error code: {1}", ae.Message, ae.ErrorCode);
                    Console.ReadLine();
                }
            };

            using (ZipArchive archive = ZipFile.OpenRead(SourceFile))
            {
                using (var db = new RenFilesEntities())
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        string nv = entry.Name;
                        var forwebtest = new UploadCapture
                        {
                            Capture = nv,
                            DateUpload = DateTime.Now
                        };
                        if (forwebtest.Id == 0)
                        {
                            try
                            {
                                db.UploadCapture.Add(forwebtest);
                            }
                            catch (Exception s)
                            {
                                s.Data.Add("info", s);
                                s.Data["ExtraInfo"] = "Information from NestedRoutine1.";
                                throw;
                            }
                        }
                        db.SaveChanges();
                    };
            };
        }

    }
}