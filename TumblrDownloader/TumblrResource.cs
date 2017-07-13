using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumblrDownloader
{
    class TumblrResource
    {
        public string ResourceName = "";
        public string ResourceURL = "";
        public string DownloadState = "";
        public string ResourceIndex = "";
        public string ResourceType = "";
        public TumblrResource(string rIndex,string rName,string rType,string rURL,string ds)
        {
            ResourceIndex = rIndex;
            ResourceName = rName;
            DownloadState = ds;
            ResourceURL = rURL;
            ResourceType = rType;
        }
    }
}
