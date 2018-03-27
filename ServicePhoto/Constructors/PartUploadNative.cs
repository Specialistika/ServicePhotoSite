using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using Constructor.UnZipFileNative;
using PhotoConsole.Domain.Data;

namespace Constructor.PartFileNative
{
    public class PartNative
    {
        private string FileName { get; set; }
        private string TempFolder { get; set; }
        private int MaxFileSizeMB { get; set; }
        private List<String> FileParts { get; set; }

        //public Utils()
        //{
        //    FileParts = new List<string>();
        //}


        public bool MergeFileNative(string FileName)
        {
            bool rslt = false;
            int FileIndex = 0;
            int FileCount = 0;
            string partToken = ".part_";
            string baseFileName = FileName.Substring(0, FileName.IndexOf(partToken));
            string trailingTokens = FileName.Substring(FileName.IndexOf(partToken) + partToken.Length);
            int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out FileIndex);
            int.TryParse(trailingTokens.Substring(trailingTokens.IndexOf(".") + 1), out FileCount);
            string Searchpattern = Path.GetFileName(baseFileName) + partToken + "*";
            string[] FilesList = Directory.GetFiles(Path.GetDirectoryName(FileName), Searchpattern);
            if (FilesList.Count() == FileCount) 
            {
                // use a singleton to stop overlapping processes
                if (!MergeFileManager.Instance.InUse(baseFileName))
                {
                    MergeFileManager.Instance.AddFile(baseFileName);
                    if (File.Exists(baseFileName))
                        File.Delete(baseFileName);
                    //add each file located to a list so we can get them into 
                    // the correct order for rebuilding the file
                    List<SortedFile> MergeList = new List<SortedFile>();
                    foreach (string File in FilesList)
                    {
                        SortedFile sFile = new SortedFile();
                        sFile.FileName = File;
                        baseFileName = File.Substring(0, File.IndexOf(partToken));
                        trailingTokens = File.Substring(File.IndexOf(partToken) + partToken.Length);
                        int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out FileIndex);
                        sFile.FileOrder = FileIndex;
                        MergeList.Add(sFile);
                    }
                    // sort by the file-part number to ensure we merge back in the correct order
                    var MergeOrder = MergeList.OrderBy(s => s.FileOrder).ToList();
                    using (FileStream FS = new FileStream(baseFileName, FileMode.Create))
                    {
                        // merge each file chunk back into one contiguous file stream
                        foreach (var chunk in MergeOrder)
                        {
                            try
                            {
                                using (FileStream fileChunk = new FileStream(chunk.FileName, FileMode.Open))
                                {
                                    fileChunk.CopyTo(FS);
                                }
                            }
                            catch (IOException ex)
                            {
                                string errorMerge = ("Error create archive" + ex.Message).Substring(0, 80);
                                using (var db = new RenFilesEntities())
                                {
                                    var Error = new ErrorFix { Error = errorMerge, DateInsert = DateTime.Now };
                                    db.ErrorFix.Add(Error);
                                    db.SaveChanges();
                                }
                            }
                            finally
                            {

                            }
                        }
                    }
                    rslt = true;
                    MergeFileManager.Instance.RemoveFile(FileName, Searchpattern);
                    UnZipNative.InteredArcive(baseFileName, null);
                }
            }
            return rslt;
        }
    }


    public struct SortedFile
    {
        public int FileOrder { get; set; }
        public String FileName { get; set; }
    }

    public class MergeFileManager
    {
        private static MergeFileManager instance;
        private List<string> MergeFileList;

        private MergeFileManager()
        {
            try
            {
                MergeFileList = new List<string>();
            }
            catch { }
        }

        public static MergeFileManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new MergeFileManager();
                return instance;
            }
        }

        public void AddFile(string BaseFileName)
        {
            MergeFileList.Add(BaseFileName);
        }

        public bool InUse(string BaseFileName)
        {
            return MergeFileList.Contains(BaseFileName);
        }

        public void RemoveFile(string FileName, string Searchpattern)
        {
            string[] fulgetpath = Directory.GetFiles(Path.GetDirectoryName(FileName), Searchpattern);
            try
            { 
            foreach (string f in fulgetpath)
            {
                File.Delete(f);
            }
            }
            catch (IOException e)
            {
                string errDelete = "Don't delete file" + e.ToString();
                using (var db = new RenFilesEntities())
                {
                    var Error = new ErrorFix { Error = errDelete, DateInsert = DateTime.Now };
                    db.ErrorFix.Add(Error);
                    db.SaveChanges();
                }
            }
                //MergeFileList.Remove(FileName);
        }
        //public unzip(string baseFileName)
        //{
        //    string result = null;
        //    UnZipNative UT = new UnZipNative();
        //    UT.InteredArcive(baseFileName);
        //    return result;
        //}
    }

}



