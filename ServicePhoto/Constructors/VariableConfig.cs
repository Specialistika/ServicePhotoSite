using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constructors.ConfigPathName;

namespace Constructors.PathString
{
    public class VariableConfig : ConfigPath
    {
        public static string ImagesPath
        {
            get { return (string)GetAppSetting(typeof(string), "ImagesPath"); }
        }
        public static string ForWebPath
        {
            get { return (string)GetAppSetting(typeof(string), "ForWebPath"); }
        }
        public static string for_mobilePath
        {
            get { return (string)GetAppSetting(typeof(string), "for_mobilePath"); }
        }
        public static string RecipePath
        {
            get { return (string)GetAppSetting(typeof(string), "RecipePath"); }
        }
        public static string NativePath
        {
            get { return (string)GetAppSetting(typeof(string), "NativePath"); }
        }
        public static string ReportPath
        {
            get { return (string)GetAppSetting(typeof(string), "ReportPath"); }
        }
        public static string ShtrihcodPath
        {
            get { return (string)GetAppSetting(typeof(string), "ShtrihcodPath"); }
        }
    }
}