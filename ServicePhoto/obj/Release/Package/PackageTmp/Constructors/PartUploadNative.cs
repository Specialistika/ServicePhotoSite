using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using Constructor.UnZipFileNative;

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
            // parse out the different tokens from the filename according to the convention
            string partToken = ".part_";
            string baseFileName = FileName.Substring(0, FileName.IndexOf(partToken));
            string trailingTokens = FileName.Substring(FileName.IndexOf(partToken) + partToken.Length);
            int FileIndex = 0;
            int FileCount = 0;
            int.TryParse(trailingTokens.Substring(0, trailingTokens.IndexOf(".")), out FileIndex);
            int.TryParse(trailingTokens.Substring(trailingTokens.IndexOf(".") + 1), out FileCount);
            // get a list of all file parts in the temp folder
            string Searchpattern = Path.GetFileName(baseFileName) + partToken + "*";
            string[] FilesList = Directory.GetFiles(Path.GetDirectoryName(FileName), Searchpattern);
            //  merge .. improvement would be to confirm individual parts are there / correctly in sequence, a security check would also be important
            // only proceed if we have received all the file chunks
            if (FilesList.Count() == FileCount) 
            {
                // use a singleton to stop overlapping processes
                if (!MergeFileManager.Instance.InUse(baseFileName))
                {
                    MergeFileManager.Instance.AddFile(baseFileName);
                    if (File.Exists(baseFileName))
                        File.Delete(baseFileName);
                    // add each file located to a list so we can get them into 
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
                                Console.WriteLine(ex.ToString());                                
                            }
                        }
                    }
                    rslt = true;
                    MergeFileManager.Instance.RemoveFile(FileName, Searchpattern);
                    MergeFileManager.Instance.unzip(baseFileName);
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

        public bool RemoveFile(string FileName, string Searchpattern)
        {
            string[] fulgetpath = Directory.GetFiles(Path.GetDirectoryName(FileName), Searchpattern);
            foreach (string f in fulgetpath)
            {
                File.Delete(f);
            }
            return MergeFileList.Remove(FileName);
        }
        public bool unzip(string baseFileName)
        {
            UnZipNative UT = new UnZipNative();
            UT.ExtractNative(baseFileName);
            return MergeFileList.Contains(baseFileName);
        }
    }

}



