using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
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
        DBOperator dbo = null;
        string currPostName = "";
        string currPostURL = "";
        string currNumPostID = "";
        string archivePage = "";

        public event EventHandler ParseOnePost;
        private void Test()
        {
            if (ParseOnePost != null)
            {
                ParseOnePost(this,new EventArgs());
            }
        }
        public TumblrDocParse(String pAddr, String port, Int32 tOut)
        {

            System.GC.Collect();
            tumDownProxy = new TDownProxy(pAddr, port, tOut);
            tumImgDT = new DataTable("tumImgDT");
            tumDoc = new HtmlDocument();
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
            dbo = new DBOperator();

        }
        public void ArchiveToPosts(int i)
        {
            for (int j = 0; j < i; j++)
            {
                Console.WriteLine("当前睡觉次数：" + j.ToString());
                Test();
                Thread.Sleep(3000);
            }
        }
        public List<string> ArchiveToPosts(string tumBlogURL)
        {

            try
            {
                tumDownHttpReq = tumDownProxy.GetHttpWebRequest(tumBlogURL);
                tumDownHttpRes = (HttpWebResponse)tumDownHttpReq.GetResponse();
                int arcPageIndex = tumBlogURL.IndexOf("page/");

                archivePage = "Page" + tumBlogURL.Substring(arcPageIndex + 4, (tumBlogURL.Length - (arcPageIndex + 4)));

                Console.Write(tumDownHttpRes.StatusCode);

                tumDownSR = new StreamReader(tumDownHttpRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                tumDoc.Load(tumDownSR);
                ResDisposed();
                HtmlNodeCollection tumArcHTMLNodes = tumDoc.DocumentNode.SelectNodes(".//a[@class]");
                Console.WriteLine("*****" + tumDoc.CreateNavigator().ToString());
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
                Console.Write("错误类型" + e.Status + "，错误信息：" + e.Message + "，方法ArchiveToPost");
            }
            finally {
                ResDisposed();
            }



            archivePage = "";
          //  Test();
            return tumPostsLT;

        }
        public void PostToImgs(string tumPostURL, string parseType)
        {
            string tmp_tumImgIndex, tmp_tumResType, tmp_tumImgName, tmp_tumImgURL, tmp_tumImgSize, tmp_tumImgPostName, tmp_tumImgPostDate, tmp_tumImgPostID, tmp_tumImgPhotoSetID, tmp_tumImgHash, tmp_tumImgDownStatus, tmp_tumImgDownTime;
            //如果解析的是post，那么postName可以从tumPostURL中截取
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("当前解析的是： " + tumPostURL);
            Regex reg = new Regex(@"[0-9]{" + Program.NumericTumPostIDLen + "}");
            string numPostID = reg.Match(tumPostURL).ToString();
            if (numPostID == "")
            {
                numPostID = "tum";
            }
            Console.WriteLine("纯数字ID是：" + numPostID);

            Console.WriteLine("postName是： " + currPostName);
            try
            {
                System.Diagnostics.Stopwatch runTimeWatch = new System.Diagnostics.Stopwatch();
                runTimeWatch.Start();
                tumDownHttpReq = tumDownProxy.GetHttpWebRequest(tumPostURL);

                tumDownHttpRes = (HttpWebResponse)tumDownHttpReq.GetResponse();

                tumDownSR = new StreamReader(tumDownHttpRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                runTimeWatch.Stop();
                Console.WriteLine("读取页面工耗时：" + runTimeWatch.ElapsedMilliseconds + "毫秒");
                tumDoc.Load(tumDownSR);
                ResDisposed();


                //增加路径是否存在的判断


                tumDoc.Save(Program.PageCodeCacheFolder + numPostID + ".txt");
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


                                AddToImgDownTable(
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
                        tmp_tumImgPhotoSetID = currPostURL;
                        //tumblr图像哈希名
                        tmp_tumImgHash = "";
                        //tumblr图像下载状态
                        tmp_tumImgDownStatus = "UN";
                        //下载完成时间
                        tmp_tumImgDownTime = "";

                        AddToImgDownTable(
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

                            AddToImgDownTable(
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

                Console.Write("错误类型" + e.Status + "，错误信息：" + e.Message + "，方法PostToImg");
            }
            finally
            {
                ResDisposed();
            }


            currPostName = "";
        }
        public DataTable GetImgDownTable()
        {
            return tumImgDT;
        }
        private void AddToImgDownTable(string tumImgIndex, string tumResType, string tumImgName, string tumImgURL, string tumImgSize, string tumImgPostName, string tumImgPostDate, string tumImgPostID, string tumImgPhotoSetID, string tumImgHash, string tumImgDownStatus, string tumImgDownTime)
        {
            tumImgCount = tumImgCount + 1;
            string[] tmpRowsValue = { tumImgCount.ToString(), tumResType, tumImgName, tumImgURL, tumImgSize, tumImgPostName, tumImgPostDate, tumImgPostID, tumImgPhotoSetID, tumImgHash, tumImgDownStatus, tumImgDownTime };
            tumImgDT.Rows.Add(tmpRowsValue);
            string insertCmd = @"insert into TumblrImgsDetails(tumImgIndex,tumResType,tumImgName,tumImgURL,tumImgSize ,tumImgPostName,tumImgPostDate,tumImgPostID,tumImgPhotoSetID,tumImgHash,tumImgDownStatus ,tumImgDownTime)" +
                "values('" + tumImgCount.ToString() + "','" + tumResType + "','" + tumImgName + "','" + tumImgURL + "','" + tumImgSize + "','" + tumImgPostName + "','" + tumImgPostDate + "','" + tumImgPostID + "','" + tumImgPhotoSetID + "','" + tumImgHash + "','" + tumImgDownStatus + "','" + tumImgDownTime + "')";
            Console.WriteLine(insertCmd);
            dbo.AddImgInfoToDB(insertCmd);
            
            
        }
        private void ResDisposed()
        {

            if (tumDownSR != null)
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
        private HtmlDocument GetHTMLDoc(string htmlURL)
        {
            HtmlDocument tmp_Doc = new HtmlDocument();
            //   HttpWebRequest tumDownHttpReq = null;
            //   HttpWebResponse tumDownHttpRes = null;
            //   StreamReader tumDownSR = null;

            try
            {
                System.Diagnostics.Stopwatch runTimeWatch = new System.Diagnostics.Stopwatch();
                runTimeWatch.Start();
                HttpWebRequest tmp_HttpReq = tumDownProxy.GetHttpWebRequest(htmlURL);
                HttpWebResponse tmp_HttpRes = (HttpWebResponse)tmp_HttpReq.GetResponse();                
                Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine("读取：" + htmlURL+"--"+ tmp_HttpRes.StatusCode);
                //   后续改为根据HttpRequest获取的网页编码，设置StreamReader的编码
                StreamReader tmp_DownSR = new StreamReader(tmp_HttpRes.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));               
                tmp_Doc.Load(tmp_DownSR);
                runTimeWatch.Stop();
                Console.WriteLine("读取页面工耗时：" + runTimeWatch.ElapsedMilliseconds + "毫秒");
                tmp_DownSR.Close();
                tmp_HttpRes.Close();
                tmp_HttpReq.Abort();
                //增加路径是否存在的判断,!!改用正则去除URL中的特殊字符
                string pageFileName = "";
                if (htmlURL.StartsWith("http://"))
                    {
                    pageFileName = htmlURL.Replace("http://", "");


                } else if (htmlURL.StartsWith("https://"))
                     {
                    pageFileName = htmlURL.Replace("https://", "");
                }
               
               
                pageFileName = pageFileName.Replace(":", "");
                pageFileName = pageFileName.Replace("/", ".");
                tmp_Doc.Save(Program.PageCodeCacheFolder + pageFileName + ".txt"); 
              
                
               
            }
            catch (WebException e)
            {
                Console.Write("错误类型" + e.Status + "，错误信息：" + e.Message + "，在getHTMLDoc方法中");
            }
            finally {
                

            }
            return tmp_Doc;
        }

        public void AnalysisPost(string tumPostURL)
        {
           

           //HtmlDocument tmp_Doc = GetHTMLDoc(tumPostURL);
            //   HttpWebRequest tumDownHttpReq = null;
            //   HttpWebResponse tumDownHttpRes = null;
            //   StreamReader tumDownSR = null;

            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("=============================================================================");
            Console.WriteLine("当前分析的是： " + tumPostURL);
            Regex reg = new Regex(@"[0-9]{" + Program.NumericTumPostIDLen + "}");

            currNumPostID = reg.Match(tumPostURL).ToString();
            if (currNumPostID == "")
            {
                Console.WriteLine(tumPostURL + " 发生错误，没有发现PostID!");
               
            }
            else {

                //设置当前分析的PostURL
                currPostURL = tumPostURL;
                //设置当前分析的PostName    
                int postNameStartAt = tumPostURL.IndexOf(currNumPostID) + Program.NumericTumPostIDLen + 1;
                //根据字符串长度判断一下是不是没有postName
                if (tumPostURL.Length > postNameStartAt )
                {
                        currPostName = tumPostURL.Substring(postNameStartAt, tumPostURL.Length - postNameStartAt);
                 }
                



                Console.WriteLine("纯数字ID是：" + currNumPostID);
                Console.WriteLine("PostName是： " + currPostName);
                try
                {
                    HtmlDocument postHTMLDoc = GetHTMLDoc(tumPostURL);
                    //根据人工识别到的tumblr页面结构，尝试分析该页面使用的何种节点结构
                    HtmlNodeCollection tumDivNodes = postHTMLDoc.DocumentNode.SelectNodes(".//div[@class='photo post']");
                    HtmlNodeCollection tumPhotoSetNodes = postHTMLDoc.DocumentNode.SelectNodes(".//div[@class='photoset']");
                    HtmlNodeCollection tumSectionNodes = postHTMLDoc.DocumentNode.SelectNodes(".//section[@class='post']");
                    Console.WriteLine("tumDivNodes数量是：" + tumDivNodes.Count);
                    Console.WriteLine("tumPhotoSetNodes数量是：" + tumPhotoSetNodes.Count);
                    Console.WriteLine("tumPostNodes数量是：" + tumSectionNodes.Count);

                    if (tumDivNodes!=null)
                    {

                        foreach (HtmlNode tmpHn in tumDivNodes)
                        {
                            //photoset 
                            if (tmpHn.Attributes["class"].Value == "html_photoset")
                            {
                                //postType="photoset";
                                if (tmpHn.ChildNodes["iframe"] != null)
                                {

                                    Console.WriteLine(tumPostURL + "  是PhotoSet");
                                    string photoSetURL = tumPostURL.Substring(0, tumPostURL.IndexOf("post") - 1) + tmpHn.ChildNodes["iframe"].Attributes["src"].Value;
                                    //方法递归调用
                                    GetPhotoSetImgs(photoSetURL);
                                }
                            }//photo post 
                            else if (tmpHn.Attributes["class"].Value == "photo post")
                            {
                                //  Console.Write(tmpHn.ChildNodes["div"].InnerHtml + "\n\r");
                                if (tmpHn.ChildNodes["div"].Attributes["class"].Value == "photo-url")
                                {
                                    HtmlNodeCollection photosNodes = tmpHn.SelectNodes(".//img[@src]");
                                    if (photosNodes.Count > 0)
                                    {
                                        GetPhotoImgs(photosNodes);
                                    }
                                    else
                                    {
                                        Console.WriteLine(tumPostURL + "中含有不含img的post-url");
                                    }
                                }


                            } // video
                            else if (tmpHn.Attributes["class"].Value == "video-player")
                            {
                                // video处理方法
                                if (tmpHn.ChildNodes["iframe"].Attributes["src"] != null)
                                {
                                    Console.WriteLine(tumPostURL + "  是Video!");
                                    string tumVideoIframeURL = tmpHn.ChildNodes["iframe"].Attributes["src"].Value;
                                    GetDivVideo(tumVideoIframeURL);

                                }


                            }
                        }
                    }
                    if (tumSectionNodes != null)
                    {
                        foreach (HtmlNode tmpHn in tumSectionNodes)
                        {
                            //photo  
                            if (tmpHn.Attributes["class"].Value == "top media")
                            {
                             
                                if (tmpHn.HasChildNodes)
                                {
                                 
                                    Console.WriteLine(tumPostURL + "  是HTML5");
                                    HtmlNodeCollection photosNodes =tmpHn.SelectNodes(".//img[@src]");
                                    if (photosNodes.Count>0)
                                    {
                                        GetPhotoImgs(photosNodes);
                                    }
                                    else
                                    {
                                        Console.WriteLine(tumPostURL + "中含有不含img的post-url");
                                    }
                                }


                            } // video，未人工识别架构
                            else if (tmpHn.Attributes["class"].Value == "video-player")
                            {
                                // video处理方法
                                if (tmpHn.ChildNodes["iframe"].Attributes["src"] != null)
                                {
                                    Console.WriteLine(tumPostURL + "  是Video!");
                                    string tumVideoIframeURL = tmpHn.ChildNodes["iframe"].Attributes["src"].Value;
                                    GetDivVideo(tumVideoIframeURL);

                                }


                            }
                        }


                    }




                }
                catch (WebException e)
                {
                    Console.Write("错误类型" + e.Status + "，错误信息：" + e.Message + "，在getHTMLDoc方法中");
                }
                finally {
                    ResDisposed();
                }
                currPostName = "";
                currPostURL = "";
              

            }
        }


        //try catch
        private void GetDivVideo(string divVideoURL)
        {
            string tmp_tumImgIndex, tmp_tumResType, tmp_tumImgName, tmp_tumImgURL, tmp_tumImgSize, tmp_tumImgPostName, tmp_tumImgPostDate, tmp_tumImgPostID, tmp_tumImgPhotoSetID, tmp_tumImgHash, tmp_tumImgDownStatus, tmp_tumImgDownTime;
            HtmlDocument tmp_Doc = GetHTMLDoc(divVideoURL);
            HtmlNodeCollection tumVideoNodes = tmp_Doc.DocumentNode.SelectNodes(".//video[@preload]");
            if (tumVideoNodes != null)
            {
                foreach (HtmlNode tmpHn in tumVideoNodes)
                {
                    if (tmpHn.ChildNodes["source"] != null)
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
                        tmp_tumImgPhotoSetID = currPostURL;
                        //tumblr图像哈希名
                        tmp_tumImgHash = "";
                        //tumblr图像下载状态
                        tmp_tumImgDownStatus = "UN";
                        //下载完成时间
                        tmp_tumImgDownTime = "";

                        AddToImgDownTable(
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
        //  try catch

        //try catch
        private void GetPhotoSetImgs(string photoSetURL)
        {
            string tmp_tumImgIndex, tmp_tumResType, tmp_tumImgName, tmp_tumImgURL, tmp_tumImgSize, tmp_tumImgPostName, tmp_tumImgPostDate, tmp_tumImgPostID, tmp_tumImgPhotoSetID, tmp_tumImgHash, tmp_tumImgDownStatus, tmp_tumImgDownTime;
            HtmlDocument tmp_Doc = GetHTMLDoc(photoSetURL);
            HtmlNodeCollection tumPhotoNodes = tmp_Doc.DocumentNode.SelectNodes(".//div[@class]");
            foreach (HtmlNode tmpHn in tumPhotoNodes)
            {
                if (tmpHn.Attributes["class"].Value.StartsWith("photoset_row"))
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
                        tmp_tumImgPhotoSetID = currPostURL.Substring(0, photoSetURL.IndexOf("post") + 5 + Program.NumericTumPostIDLen);
                        //tumblr图像哈希名
                        tmp_tumImgHash = "";
                        //tumblr图像下载状态
                        tmp_tumImgDownStatus = "UN";
                        //下载完成时间
                        tmp_tumImgDownTime = "";

                        AddToImgDownTable(
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
        // try catch
        private void GetPhotoImgs(HtmlNodeCollection photosNodes)
        {
            string tmp_tumImgIndex, tmp_tumResType, tmp_tumImgName, tmp_tumImgURL, tmp_tumImgSize, tmp_tumImgPostName, tmp_tumImgPostDate, tmp_tumImgPostID, tmp_tumImgPhotoSetID, tmp_tumImgHash, tmp_tumImgDownStatus, tmp_tumImgDownTime;
            if(photosNodes!=null)
            {
                foreach (HtmlNode tmpHn in photosNodes)
                {
                    // Console.Write(photosNodes[0].InnerHtml);
                    //图像编号
                    tmp_tumImgIndex = "";
                    //图像类型		
                    tmp_tumResType = "photo";
                    //tumblr图像名
                    tmp_tumImgName = "";
                    //tumblr图像URL
                    //Console.Write(tmpHn.InnerHtml);
                    tmp_tumImgURL = tmpHn.Attributes["src"].Value;
                    //tmpHn.ChildNodes["div"].ChildNodes["img"].Attributes["src"].Value;
                    //tumblr图像大小
                    tmp_tumImgSize = "";
                    //tumblr图像PostName
                    tmp_tumImgPostName = currPostName;
                    //tumblr图像上传日期
                    tmp_tumImgPostDate = "";
                    //tumblr图像postID
                    tmp_tumImgPostID = currPostURL;
                    //tumblr图像photoSetID
                    tmp_tumImgPhotoSetID = "";
                    //tumblr图像哈希名
                    tmp_tumImgHash = "";
                    //tumblr图像下载状态
                    tmp_tumImgDownStatus = "UN";
                    //tumblr图像下载时间
                    tmp_tumImgDownTime = "";


                    AddToImgDownTable(
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
}
