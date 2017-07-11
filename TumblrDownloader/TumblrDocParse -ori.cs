using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Data;
using System.Net;
using System.IO;

namespace TumblrDownloader
{
    class TumblrDocParse
    {
        HtmlAgilityPack.HtmlDocument tumDoc;
        TDownProxy tumDownProxy;
        DataTable tumImgDT;
        List<string> tumPostsLT;
        Int32 tumImgCount = 0;

        HttpWebRequest tumDownHttpReq = null;
        HttpWebResponse tumDownHttpRes = null;
        StreamReader tumDownSR = null;
        string currPostName = "";
        public TumblrDocParse(String pAddr, String port, Int32 tOut)
        {
            
            System.GC.Collect();
            tumDownProxy =  new TDownProxy(pAddr,port,tOut); 
            tumImgDT = new DataTable("tumImgDT");
            tumDoc =new HtmlDocument();
            tumPostsLT = new List<string>();
            tumImgDT.Columns.Add("tumImgIndex");
            tumImgDT.Columns.Add("tumResType");
            tumImgDT.Columns.Add("tumImgName");
            tumImgDT.Columns.Add("tumImgURL");
            tumImgDT.Columns.Add("tumImgSize");
            tumImgDT.Columns.Add("tumImgPostName");
            tumImgDT.Columns.Add("tumImgPostDate");
            tumImgDT.Columns.Add("tumImgPostID");
            tumImgDT.Columns.Add("tumImgPhotoSetID");
            tumImgDT.Columns.Add("tumImgHash");
            tumImgDT.Columns.Add("tumImgDownStatus");
            tumImgDT.Columns.Add("tumImgDownTime");
            
        }

        public List<string> ArchiveToPosts(string tumBlogURL)
        {

            try
            {
                tumDownHttpReq = tumDownProxy.GetHttpWebRequest(tumBlogURL);
                tumDownHttpRes = (HttpWebResponse)tumDownHttpReq.GetResponse();

                    Console.Write(tumDownHttpRes.StatusCode);

                    tumDownSR = new StreamReader(tumDownHttpRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                    tumDoc.Load(tumDownSR);
                    ResDisposed();
                    HtmlNodeCollection tumArcHTMLNodes = tumDoc.DocumentNode.SelectNodes(".//a[@class]");
                Console.WriteLine("*****"+ tumDoc.CreateNavigator().ToString());
                    foreach (HtmlNode tmpHn in tumArcHTMLNodes)
                    {
                        if (tmpHn.Attributes["class"].Value == "hover")
                        {
                            tumPostsLT.Add(tmpHn.Attributes["href"].Value);
                        }

                    }
                
            }
            catch (WebException e)
            {
                Console.Write("错误类型"+e.Status +"，错误信息："+ e.Message+"，方法ArchiveToPost");
            }
            finally {
                ResDisposed();
            }
           
            
           
            
            return tumPostsLT;

        }
        public void PostToImgs(string tumPostURL,string parseType)
        {
            string tmp_tumImgIndex,tmp_tumResType,tmp_tumImgName, tmp_tumImgURL, tmp_tumImgSize, tmp_tumImgPostName,tmp_tumImgPostDate, tmp_tumImgPostID, tmp_tumImgPhotoSetID,tmp_tumImgHash, tmp_tumImgDownStatus,tmp_tumImgDownTime;
            //如果解析的是post，那么postName可以从tumPostURL中截取
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("当前解析的是： " + tumPostURL);
            Regex reg = new Regex(@"[0-9]{"+Program.NumericTumPostIDLen+"}");
            string numPostID = reg.Match(tumPostURL).ToString();
            if (numPostID == "")
            {
                numPostID = "tum";
            }
            Console.WriteLine("纯数字ID是：" + numPostID );

            Console.WriteLine("postName是： "+currPostName);
            try
            {
                System.Diagnostics.Stopwatch runTimeWatch = new System.Diagnostics.Stopwatch();
                runTimeWatch.Start();
                tumDownHttpReq = tumDownProxy.GetHttpWebRequest(tumPostURL);     

                tumDownHttpRes = (HttpWebResponse)tumDownHttpReq.GetResponse();
                
                tumDownSR = new StreamReader(tumDownHttpRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                runTimeWatch.Stop();
                Console.WriteLine("读取页面工耗时："+runTimeWatch.ElapsedMilliseconds+"毫秒");
                tumDoc.Load(tumDownSR);
                ResDisposed();


                //增加路径是否存在的判断


                tumDoc.Save(Program.PageCodeCacheFolder+numPostID+".txt");
                HtmlNodeCollection tumPostHTMLNodes = tumDoc.DocumentNode.SelectNodes(".//div[@class]");
                if (parseType == "post")
                {
                  
                    int postIndex = tumPostURL.IndexOf("post") + 4 + Program.NumericTumPostIDLen + 1;
                    //根据字符串长度判断一下是不是没有postName
                    if (tumPostURL.Length > (postIndex + 1))
                    {
                        currPostName = tumPostURL.Substring(postIndex + 1, tumPostURL.Length - (postIndex + 1));
                    }
                }
                //如果解析的是photoset，那么postName不变
                else if (parseType == "photoset")
                {
                    
                }
                else if (parseType == "video")
                {
                 
                    tumPostHTMLNodes = tumDoc.DocumentNode.SelectNodes(".//video[@preload]");
                }
                foreach (HtmlNode tmpHn in tumPostHTMLNodes)
                {

                    //  Console.Write(tmpHn.Attributes["class"].Value + "  className");
                    if (tmpHn.Attributes["class"].Value == "html_photoset")
                    {
                        if (tmpHn.ChildNodes["iframe"] != null)
                        {

                            Console.WriteLine(tumPostURL + "  是PhotoSet");
                            string photoSetURL = tumPostURL.Substring(0, tumPostURL.IndexOf("post") - 1) + tmpHn.ChildNodes["iframe"].Attributes["src"].Value;
                            //方法递归调用
                            PostToImgs(photoSetURL, "photoset");


                        }

                        // tumPostsLT.Add(tmpHn.Attributes["href"].Value);
                        // photoset处理方法
                    }
                    else if (tmpHn.Attributes["class"].Value == "photo post")
                    {
                        //  Console.Write(tmpHn.ChildNodes["div"].InnerHtml + "\n\r");
                        if (tmpHn.ChildNodes["div"].Attributes["class"].Value == "photo-url")
                        {
                            HtmlNodeCollection photosNodes = tmpHn.SelectNodes(".//img[@src]");
                            if (photosNodes.Count > 0)
                            {

                                //Console.Write(photosNodes[0].InnerHtml);
                                //图像编号
                                tmp_tumImgIndex = "";
                                //图像类型
                                tmp_tumResType = "photo";
                                //tumblr图像名
                                tmp_tumImgName = "";
                                //tumblr图像URL
                                //Console.Write(tmpHn.InnerHtml);
                                tmp_tumImgURL = photosNodes[0].Attributes["src"].Value;
                                //tmpHn.ChildNodes["div"].ChildNodes["img"].Attributes["src"].Value;
                                //tumblr图像大小
                                tmp_tumImgSize = "";
                                //tumblr图像PostName
                                tmp_tumImgPostName = currPostName;
                                //tumblr图像上传日期
                                tmp_tumImgPostDate = "";
                                //tumblr图像postID
                                tmp_tumImgPostID = tumPostURL;
                                //tumblr图像photoSetID
                                tmp_tumImgPhotoSetID = "";
                                //tumblr图像哈希名
                                tmp_tumImgHash = "";
                                //tumblr图像下载状态
                                tmp_tumImgDownStatus = "UN";
                                tmp_tumImgDownTime = "";


                                AddToTumIngDownTable(
                                 //图像编号
                                 tmp_tumImgIndex,
                                //图像类型
                                tmp_tumResType,
                                //图像名称
                                tmp_tumImgName,
                                //tumblr图像URL
                                tmp_tumImgURL,
                                //tumblr图像大小
                                tmp_tumImgSize,
                                //tumblr图像PostName
                                tmp_tumImgPostName,
                                //tumblr图像上传日期
                                tmp_tumImgPostDate,
                                //tumblr图像postID
                                tmp_tumImgPostID,
                                //tumblr图像photosetID
                                tmp_tumImgPhotoSetID,
                                //tumblr图像哈希名
                                tmp_tumImgHash,
                                //tumblr图像下载状态
                                tmp_tumImgDownStatus,
                                //下载完成时间
                                tmp_tumImgDownTime
                                );
                            }
                            else
                            {
                                Console.WriteLine(tumPostURL + "中含有不含img的post-url");
                            }
                        }


                    }
                    else if (tmpHn.Attributes["class"].Value == "video-player")
                    {

                        // video处理方法
                        if (tmpHn.ChildNodes["iframe"].Attributes["src"] != null)
                        {
                            Console.WriteLine(tumPostURL + "  是Video");
                            string tumVideoIframeURL = tmpHn.ChildNodes["iframe"].Attributes["src"].Value;
                            PostToImgs(tumVideoIframeURL, "video");

                        }


                    }
                    else if (tmpHn.ChildNodes["source"] != null)
                    {
                        //图像编号
                        tmp_tumImgIndex = "";
                        //图像类型
                        tmp_tumResType = "video";
                        //tumblr图像名
                        tmp_tumImgName = "";
                        //tumblr图像URL
                        //Console.WriteLine(tmpHn.InnerHtml);
                        tmp_tumImgURL = tmpHn.ChildNodes["source"].Attributes["src"].Value;
                        //tumblr图像大小
                        tmp_tumImgSize = "";
                        //tumblr图像PostName
                        tmp_tumImgPostName = currPostName;
                        //tumblr图像上传日期
                        tmp_tumImgPostDate = "";
                        //tumblr图像postID
                        tmp_tumImgPostID = "";
                        //tumblr图像photoSetID                          
                         tmp_tumImgPhotoSetID= tumPostURL ;
                            //tumblr图像哈希名
                            tmp_tumImgHash = "";
                            //tumblr图像下载状态
                            tmp_tumImgDownStatus = "UN";
                            //下载完成时间
                            tmp_tumImgDownTime = "";

                            AddToTumIngDownTable(
                             //图像编号
                             tmp_tumImgIndex,
                            //图像类型
                             tmp_tumResType,
                            //图像名称
                            tmp_tumImgName,
                            //tumblr图像URL
                            tmp_tumImgURL,
                            //tumblr图像大小
                            tmp_tumImgSize,
                            //tumblr图像PostName
                            tmp_tumImgPostName,
                            //tumblr图像上传日期
                            tmp_tumImgPostDate,
                            //tumblr图像postID
                            tmp_tumImgPostID,
                            //tumblr图像photosetID
                            tmp_tumImgPhotoSetID,
                            //tumblr图像哈希名
                            tmp_tumImgHash,
                            //tumblr图像下载状态
                            tmp_tumImgDownStatus,
                            //下载完成时间
                            tmp_tumImgDownTime
                            );

                    }

                    else if (tmpHn.Attributes["class"].Value.StartsWith("photoset_row"))
                    {
                        if (tmpHn.ChildNodes["a"].Attributes["class"].Value.StartsWith("photoset_photo"))
                        {
                            //图像编号
                            tmp_tumImgIndex = "";
                            //图像类型
                            tmp_tumResType = "photo";
                            //tumblr图像名
                            tmp_tumImgName = "";
                            //tumblr图像URL
                            //Console.WriteLine(tmpHn.InnerHtml);
                            tmp_tumImgURL = tmpHn.ChildNodes["a"].Attributes["href"].Value;
                            //tumblr图像大小
                            tmp_tumImgSize = "";
                            //tumblr图像PostName
                            tmp_tumImgPostName = currPostName;
                            //tumblr图像上传日期
                            tmp_tumImgPostDate = "";
                            //tumblr图像postID
                            tmp_tumImgPostID = "";
                            //tumblr图像photoSetID
                            tmp_tumImgPhotoSetID = tumPostURL.Substring(0, tumPostURL.IndexOf("post") + 5 + Program.NumericTumPostIDLen);
                            //tumblr图像哈希名
                            tmp_tumImgHash = "";
                            //tumblr图像下载状态
                            tmp_tumImgDownStatus = "UN";
                            //下载完成时间
                            tmp_tumImgDownTime = "";

                            AddToTumIngDownTable(
                             //图像编号
                             tmp_tumImgIndex,
                            //图像类型
                             tmp_tumResType,
                            //图像名称
                            tmp_tumImgName,
                            //tumblr图像URL
                            tmp_tumImgURL,
                            //tumblr图像大小
                            tmp_tumImgSize,
                            //tumblr图像PostName
                            tmp_tumImgPostName,
                            //tumblr图像上传日期
                            tmp_tumImgPostDate,
                            //tumblr图像postID
                            tmp_tumImgPostID,
                            //tumblr图像photosetID
                            tmp_tumImgPhotoSetID,
                            //tumblr图像哈希名
                            tmp_tumImgHash,
                            //tumblr图像下载状态
                            tmp_tumImgDownStatus,
                            //下载完成时间
                            tmp_tumImgDownTime
                            );




                        }



                    }


                }
            }
            catch (WebException e)
            {

                Console.Write("错误类型" + e.Status + "，错误信息：" + e.Message+"，方法PostToImg");
            }
            finally
            {
                ResDisposed();
            }


            currPostName = "";
        }
        public DataTable GetTumImgDownTable()
        {
            return tumImgDT;
        }
        private void AddToTumIngDownTable(string tumImgIndex,string tumResType,string tumImgName,string tumImgURL,string tumImgSize, string tumImgPostName,string tumImgPostDate,string tumImgPostID,string tumImgPhotoSetID,string tumImgHash,string tumImgDownStatus,string  tumImgDownTime)
        {
            tumImgCount = tumImgCount + 1;
            string[] tmpRowsValue = { tumImgCount.ToString(),tumResType, tumImgName, tumImgURL, tumImgSize, tumImgPostName, tumImgPostDate,tumImgPostID,tumImgPhotoSetID, tumImgHash, tumImgDownStatus , tumImgDownTime };
            tumImgDT.Rows.Add(tmpRowsValue);
        }
        private void ResDisposed()
        {
           
            if (tumDownSR!=null)
            {
                tumDownSR.Close();
            }
            if (tumDownHttpRes != null)
            {
                tumDownHttpRes.Close();
            }
            if (tumDownHttpReq != null)
            {
                tumDownHttpReq.Abort();
            }
           
           
           
            tumDownHttpReq = null;
            tumDownHttpRes = null;
            tumDownSR = null;
        }
    }
}
