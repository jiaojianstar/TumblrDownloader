using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TumblrDownloader
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        public static int NumericTumPostIDLen=12;
        public static string PageCodeCacheFolder = "D:\\tumblr\\";
        public static string DBFilePath = "default";
        public static string DBFileName = "TumblrImgs.db3";
        public static string TumblrResourcesFolder = "D:\\TumblrResources\\";
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
