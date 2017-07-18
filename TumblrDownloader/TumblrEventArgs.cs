using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumblrDownloader
{
    class TumblrEventArgs:EventArgs
    {
        public string TumblrPostURL="";
        public string TumblrResourceSize = "";
        public string TumblrResourceDownloadStatus = "";
        public string TumblrResourceIndex = "";
        public string TumblrResourceTime = "";

    }
}
