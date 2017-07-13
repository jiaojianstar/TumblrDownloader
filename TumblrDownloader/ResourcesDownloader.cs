using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace TumblrDownloader
{
    class ResourcesDownloader
    {
        private TumblrResource[] ToDownRes = null;
        private TDownProxy tumDownProxy = null;
        private bool SaveResourcesToFolderWithPostName = false;

        private HttpWebRequest TumDownReq = null;
        private HttpWebResponse TumDownRes = null;
        private Stream tumStream = null;
       
        public ResourcesDownloader()
        {

        }
        public ResourcesDownloader(String pAddr, String port, Int32 tOut)
        {
            tumDownProxy = new TDownProxy(pAddr, port, tOut);
            DirectoryInfo tumDI = new DirectoryInfo(Program.TumblrResourcesFolder);
            if (!tumDI.Exists)
            {
                tumDI.Create();
            }

        }
        public void StartDownLoad()
        {
            if (ToDownRes.Count<TumblrResource>() > 0)
            {
                foreach (TumblrResource tr in ToDownRes)
                {
                    String tumResFilePath = Program.TumblrResourcesFolder + tr.ResourceName;
                    FileInfo tumResFileInfo = new FileInfo(tumResFilePath);
                    if (!tumResFileInfo.Exists)
                    {

                        if (true)
                        {
                            try
                            {
                                TumDownReq = tumDownProxy.GetHttpWebRequest(tr.ResourceURL);
                                TumDownRes = (HttpWebResponse)TumDownReq.GetResponse();
                                byte[] imageBuffer = new byte[10240];
                                if (TumDownRes.StatusCode == HttpStatusCode.OK)
                                {

                                    tumStream = TumDownRes.GetResponseStream();
                                    Console.WriteLine("资源长度： "+TumDownRes.ContentLength);
                                    int readSize = tumStream.Read(imageBuffer, 0, imageBuffer.Length);
                                    int filelenth = 0;
                                    FileStream tumFS  = new FileStream(tumResFilePath, FileMode.Create);
                                    while (readSize > 0)
                                    {
                                        tumFS.Write(imageBuffer, 0, readSize);
                                        filelenth = filelenth + readSize;
                                        readSize = tumStream.Read(imageBuffer, 0, imageBuffer.Length);
                                        Console.WriteLine("已写入长度： "+tumFS.Length);
                                    }

                                   
                                    tumFS.Close();



                                }
                            }
                            catch (Exception e)
                            {

                            }
                            finally
                            {

                               
                                tumStream.Close();
                                TumDownRes.Close();
                                TumDownReq.Abort();
                            }

                        }
                        /*
                        else if (tr.ResourceType == "video")
                        {

                        }
                        */
                    }
                    else
                    {

                        //文件已存在

                    }
                }

                


            }

        }
        public void SetDownloadResources(TumblrResource[] tdr)
        {
            ToDownRes = tdr;

        }
    }
}
