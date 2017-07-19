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
            Console.WriteLine(System.Threading.Thread.CurrentThread.Name+"线程待下载资源数量是：" + ToDownRes.Count);
            if (ToDownRes.Count<TumblrResource>() > 0)
            {
                
                foreach (TumblrResource tr in ToDownRes)
                {
                    String tumResFilePath = Program.TumblrResourcesFolder + tr.ResourceName;
                    FileInfo tumResFileInfo = new FileInfo(tumResFilePath);
                    long tumContentLength = 0;
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
                                    tumContentLength = TumDownRes.ContentLength;
                                    int tumResSize_KB = (int)Math.Round((double)tumContentLength / 1024, 0);
                                    double tumResSize_MB = Math.Round((double)tumContentLength / 1024 / 1024, 2);
                                    if (tumResSize_KB < 10240)
                                    {
                                        TumblrResourceSize = tumResSize_KB + " KB";

                                    }
                                    else
                                    {
                                        TumblrResourceSize = tumResSize_MB + " MB";
                                    }

                                    Console.WriteLine("编号为"+tr.ResourceIndex +"资源长度： " + TumblrResourceSize);
                                    int readSize = tumStream.Read(imageBuffer, 0, imageBuffer.Length);
                                    int filelenth = 0;
                                    FileStream tumFS = new FileStream(tumResFilePath, FileMode.Create);
                                    while (readSize > 0)
                                    {
                                        tumFS.Write(imageBuffer, 0, readSize);
                                        filelenth = filelenth + readSize;
                                        readSize = tumStream.Read(imageBuffer, 0, imageBuffer.Length);
                                        Console.WriteLine(System.Threading.Thread.CurrentThread.Name + "已写入长度： " + tumFS.Length);
                                    }


                                    tumFS.Close();
                                    tumStream.Close();
                                    TumblrEventArgs tea = new TumblrEventArgs();


                                    dbo.UpdateTumblrResourceItem(tr.ResourceIndex, tumContentLength.ToString(), DateTime.Now.ToString());
                                    tea.TumblrResourceDownloadStatus = "DN";
                                    tea.TumblrResourceIndex = tr.ResourceIndex;
                                    tea.TumblrResourceSize = this.TumblrResourceSize;
                                    tea.TumblrResourceTime = DateTime.Now.ToString();
                                    CallOneResourceDownloadedEvent(tea);
                                }
                            }
                            catch (IOException e)
                            {
                                Console.WriteLine(System.Threading.Thread.CurrentThread.Name + "下载："+ tr.ResourceIndex + "，发生IO异常:" + e.Message);
                            }
                            catch (WebException e)
                            {
                                Console.WriteLine(System.Threading.Thread.CurrentThread.Name + "下载：" + tr.ResourceIndex + "，发生NET异常:" + e.Message);
                            }
                            finally
                            {



                                if (TumDownRes != null)
                                {
                                    TumDownRes.Close();
                                }
                                if (TumDownReq != null)
                                {
                                    TumDownReq.Abort();
                                }

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
