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
        //private TumblrResource[] ToDownRes = null;
        private List<TumblrResource> ToDownRes = null;
        private TDownProxy tumDownProxy = null;
        private bool SaveResourcesToFolderWithPostName = false;
        private string TumblrResourceSize ="";
        private HttpWebRequest TumDownReq = null;
        private HttpWebResponse TumDownRes = null;
        private Stream tumStream = null;
        public event EventHandler<TumblrEventArgs> OnOneResourceDownloaded;
        private DBOperator dbo = null;
        private void CallOneResourceDownloadedEvent(TumblrEventArgs e)
        {
            if (OnOneResourceDownloaded != null)
            {            

                OnOneResourceDownloaded.Invoke(this, e);
            }
        }
        public ResourcesDownloader()
        {

        }
        public ResourcesDownloader(String pAddr, String port, Int32 tOut)
        {
            tumDownProxy = new TDownProxy(pAddr, port, tOut);
            DirectoryInfo tumDI = new DirectoryInfo(Program.TumblrResourcesFolder);
            dbo = new DBOperator();
           
            if (!tumDI.Exists)
            {
                tumDI.Create();
            }

        }
        public void StartDownLoad()
        {
            Console.WriteLine("当前待下载资源数量是：" + ToDownRes.Count);
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
                                byte[] imageBuffer = new byte[Program.TumblrResourceBufferSize];
                                if (TumDownRes.StatusCode == HttpStatusCode.OK)
                                {

                                    tumStream = TumDownRes.GetResponseStream();
                                    int tumResSize_KB =(int)Math.Round( (double)TumDownRes.ContentLength / 1024,0);
                                    double tumResSize_MB = Math.Round((double)TumDownRes.ContentLength / 1024 / 1024, 2);
                                    if (tumResSize_KB < 10240)
                                    {
                                        TumblrResourceSize = tumResSize_KB + " KB";

                                    }
                                    else {
                                        TumblrResourceSize = tumResSize_MB + " MB";
                                    }
                                  
                                    Console.WriteLine("资源长度： "+ TumblrResourceSize);
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
                                    tumStream.Close();
                                    TumblrEventArgs tea = new TumblrEventArgs();


                                    dbo.UpdateTumblrResourceItem(tr.ResourceIndex, this.TumblrResourceSize, DateTime.Now.ToString());
                                    tea.TumblrResourceDownloadStatus = "DN";
                                    tea.TumblrResourceIndex = tr.ResourceIndex;
                                    tea.TumblrResourceSize = this.TumblrResourceSize;
                                    tea.TumblrResourceTime = DateTime.Now.ToString();
                                    CallOneResourceDownloadedEvent(tea);
                                }
                            }
                            catch (Exception e)
                            {

                            }
                            finally
                            {

                                
                               
                               
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
        public void SetDownloadResources(List<TumblrResource> tdr)
        {
            ToDownRes = tdr;

        }
    }
}
