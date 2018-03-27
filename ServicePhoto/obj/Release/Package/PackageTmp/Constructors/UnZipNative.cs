using System;
using System.IO;
using ComponentAce.Compression.ZipForge;
using ComponentAce.Compression.Archiver;
using System.IO.Compression;
using PhotoConsole.Domain.Data;
using Constructors.PathString;

namespace Constructor.UnZipFileNative
{
    public class UnZipNative
    {
              //public string SourceFile { get; set; }

              public void ExtractNative(string SourceFile)
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
                      using (var db = new RenFilesEntities1())
                          foreach (ZipArchiveEntry entry in archive.Entries)
                          {
                              string nv = entry.Name;
                              var forwebtest = new IntoCapture 
                              { 
                                  ToCapture = nv, 
                                  Dateloginto = DateTime.Now 
                              };
                              if (forwebtest.Id == 0)
                              {
                                  try
                                  {
                                      db.IntoCapture.Add(forwebtest);
                                  }
                                  catch (Exception s)
                                  {
                                      s.Data.Add("info", s);
                                      s.Data["ExtraInfo"] = "Information from NestedRoutine1.";
                                      throw;
                                  }
                              };
                              db.SaveChanges();
                          }
                  };
              }
    }
}