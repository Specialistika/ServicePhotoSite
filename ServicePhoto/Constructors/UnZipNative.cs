using System;
using System.IO;
using System.IO.Compression;
using PhotoConsole.Domain.Data;
using System.Linq;

namespace Constructor.UnZipFileNative
{
    public static class UnZipNative
    {
        public static void InteredArcive(string SourceFile, string NameFolder)
        {
			string fileName = Path.GetFileName(SourceFile);

			#region extract archive
			//using (var archiver = new ZipForge())
			//{
			//    try
			//    {
			//        archiver.FileName = SourceFile;
			//        archiver.OpenArchive(System.IO.FileMode.Open);
			//        archiver.BaseDir = VariableConfig.ForWebPath;
			//        archiver.ExtractFiles("*.*");
			//        archiver.CloseArchive();
			//    }
			//    catch (ArchiverException ae)
			//    {
			//        ErroreArchive = "Message: {0}\t Error code: {1}" + ae.Message + ae.ErrorCode;
			//    }
			//};
			#endregion

			if (SourceFile.Contains(".zip"))
			{
				using (var db = new RenFilesEntities())
				{
					var NameZipfile = new NameZip { NamefileZip = fileName, DateTime = DateTime.Now };
					db.NameZip.Add(NameZipfile);
					db.SaveChanges();

					using (var archive = ZipFile.OpenRead(SourceFile))
					{
						foreach (ZipArchiveEntry entry in archive.Entries)
						{
							var nameCapture = new UploadCapture
							{
								Id_NameZip = db.NameZip.Max(s => s.Id),
								Capture = entry.Name,
								DateUpload = DateTime.Now,
								FolderName = NameFolder
							};
							if (nameCapture.Id == 0)
							{
								try
								{
									db.UploadCapture.Add(nameCapture);
								}
								catch (Exception s)
								{
									string ErroreArchive = "ErrorArchive " + s.Message;
									var Error = new ErrorFix { Error = ErroreArchive, DateInsert = DateTime.Now };
									db.ErrorFix.Add(Error);
								}
							};
						}
						db.SaveChanges();
					};
				};
			}
			else
			{
				using (var db = new RenFilesEntities())
				{
					try
					{
						var nameCapture = new UploadCapture
						{
							Capture = fileName,
							DateUpload = DateTime.Now,
							FolderName = NameFolder
						};

						db.UploadCapture.Add(nameCapture);
					}
					catch (Exception s)
					{
						string ErroreArchive = "ErrorArchive " + s.Message;
						var Error = new ErrorFix { Error = ErroreArchive, DateInsert = DateTime.Now };
						db.ErrorFix.Add(Error);
					}
					db.SaveChanges();
				}
			}
        }
    }
}