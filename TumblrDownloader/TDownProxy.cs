using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
namespace TumblrDownloader
{
    class TDownProxy
    {
        
        WebProxy wProxy;
        Int32 pPort;
        Int32 timeOut;
        
        public  TDownProxy(String pAddr, String port,Int32 tOut)
        {
            pPort = Int32.Parse(port);
            wProxy = new WebProxy(pAddr, pPort);
            timeOut = tOut;

        }
        public HttpWebRequest GetHttpWebRequest(string tURI)
        {
            HttpWebRequest hwr = (HttpWebRequest)WebRequest.Create(tURI);
            hwr.Proxy = wProxy;
            hwr.ServicePoint.Expect100Continue = false;
            hwr.Timeout = timeOut;
            hwr.Method = "get";
            hwr.UserAgent="Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.86 Safari/537.36";
            hwr.ContentType= "text/html; charset=utf-8";
            hwr.KeepAlive = true;
            hwr.Accept = "*/*";
          
            hwr.ServicePoint.ConnectionLimit = 100000;


            return hwr;
        }
        public void Close()
        {
          
        }
    }
}
