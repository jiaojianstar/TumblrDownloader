using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;
using System.IO;
namespace TumblrDownloader
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            tumDP = new TumblrDocParse();
            tumDP.ParseOnePost += UpdateStatusLable;

        }
        TumblrDocParse tumDP;
        public delegate void Myinvoke(EventArgs str);

        private void work(object obj, DoWorkEventArgs e)
        {
            MessageBox.Show(e.ToString());
            tumDP.ArchiveToPosts(100);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            tumDP = new TumblrDocParse(proxyAddr.Text, proxyPort.Text, 10000);
          
            //  BackgroundWorker bw = new BackgroundWorker();
            //  bw.DoWork +=new DoWorkEventHandler(work);
            //  bw.RunWorkerAsync(blogName.Text);
            //tumDP.ArchiveToPosts(blogName.Text);


            List<string> tumPL=tumDP.ArchiveToPosts(blogName.Text);
            LBL_PostCNT.Text = tumPL.Count + "条";
            LBL_PostCNT.Update();
           
            
            for(int postIndex=0;postIndex<tumPL.Count;postIndex++)
            { 
                Console.WriteLine("******当前post是："+postIndex);
                
               
                ListViewItem lvi = new ListViewItem();
                lvi.SubItems.Add(postIndex.ToString());
                lvi.SubItems.Add(tumPL[postIndex].ToString());
                lvi.SubItems.Add("");
                lvi.SubItems.Add("");
                listView1.Items.Add(lvi);
                listView1.Update();
                // tumDP.PostToImgs(tmpStr,"post");
                tumDP.AnalysisPost(tumPL[postIndex].ToString());
               
                
            }
            
            
             


            
            dataGridView1.DataSource = tumDP.GetImgDownTable();
            
            /**
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
            ***/


            dataGridView1.Columns["tumImgIndex"].HeaderText = "序号";
            dataGridView1.Columns["tumImgIndex"].Width = 75;
            dataGridView1.Columns["tumResType"].HeaderText = "类型";
            dataGridView1.Columns["tumResType"].Width= 75;
            dataGridView1.Columns["tumImgName"].HeaderText = "文件名";
            dataGridView1.Columns["tumResourceURL"].HeaderText = "资源URL";
            dataGridView1.Columns["tumResourceURL"].Width = 700;
            dataGridView1.Columns["tumImgSize"].HeaderText = "大小";
            dataGridView1.Columns["tumImgPostName"].HeaderText = "Post名称";
            dataGridView1.Columns["tumImgPostDate"].HeaderText = "Post日期";
            dataGridView1.Columns["tumImgPostID"].HeaderText = "PostID";
            dataGridView1.Columns["tumNumbericPostID"].HeaderText = "纯数字PostID";
            dataGridView1.Columns["tumImgHash"].HeaderText = "哈希";
            dataGridView1.Columns["tumImgDownStatus"].HeaderText = "状态";
            dataGridView1.Columns["tumImgDownTime"].HeaderText = "下载完成时间";

            /**
            HttpWebRequest tDwnReq = tdp.getHttpWebRequest( tmpHNC.Attributes["data-imageurl"].Value) ;
            
            HttpWebResponse tDwnRes= tDwnReq.GetResponse() as HttpWebResponse;
            Stream responseStream =tDwnRes.GetResponseStream();
            Stream stream = new FileStream("c:\\t\\"+i+".jpg", FileMode.Create);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, (int)bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, (int)bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            i++;
            **/
        }


        //   Application.Exit();







        

        public void UpdateStatusLable(Object o, EventArgs e)
        {
            Myinvoke m = new Myinvoke(Method);

            if(label5.InvokeRequired)
            {
               
                // MessageBox.Show(ex.TumblrPostURL);
                label5.Invoke(m,new object[] {e});
               

            }
            
           


        }
        private void Method(EventArgs e)
        {
            TumblrParseEventArgs ex = (TumblrParseEventArgs)e;
            //  MessageBox.Show(ex.TumblrPostURL);
            ListViewItem lvi = new ListViewItem(listView1.Items.Count.ToString());
          //  MessageBox.Show(listView1.Items.Count.ToString());
            listView1.Columns[0].Name = "tumPostIndex";
            listView1.Columns[1].Name = "tumPostID";
            listView1.Columns[2].Name = "tumPostURL";
            listView1.Columns[3].Name = "tumResCnt";
            listView1.Columns[4].Name = "parseErrors";
          //  lvi.SubItems.Add(listView1.Items.Count.ToString());
            lvi.SubItems.Add("未完成");
            lvi.SubItems.Add(ex.TumblrPostURL);
            lvi.SubItems.Add("未完成");
            lvi.SubItems.Add("未完成");

            listView1.Columns["tumPostIndex"].Text = "序号";
            listView1.Columns["tumPostID"].Text = "PostID";
            listView1.Columns["tumPostURL"].Text ="PostURL";
            listView1.Columns["tumResCnt"].Text ="资源数量" ;
            listView1.Columns["parseErrors"].Text ="错误";
           
            listView1.Items.Add(lvi);
            // label5.Text = ex.TumblrPostURL;
            // label5.Update();
            listView1.Update();
        }
        

        
            

        
        
        private void button2_Click(object sender, EventArgs e)
        {
            //  Clipboard.SetDataObject(checkedListBox1.SelectedItem.ToString());

            // tumDP = new TumblrDocParse(proxyAddr.Text, proxyPort.Text, 10000);
            tumDP.SetProxyParas(proxyAddr.Text, proxyPort.Text, 10000);
            tumDP.SetTumblrBlogName(blogName.Text);          
            Thread t = new Thread(tumDP.RunTumbleAnalysis);
            t.Start();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
            for (int j = 0; j < dataGridView1.Rows.Count ; j++)
            {
                dataGridView1.Rows[j].Visible = true;
            }
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            /*
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetSelected(i))
                {
                    dataGridView1.CurrentCell = null;
                    
                    string tmpPostURL = checkedListBox1.Items[i].ToString();
                    int tmpPostIndex = tmpPostURL.IndexOf("post") + 4 +1+ Program.NumericTumPostIDLen;
                    string tmpPostID = tmpPostURL.Substring(0, tmpPostIndex);
                    string dgvPostID = "";
                    for (int j = 0; j < dataGridView1.Rows.Count-1; j++)
                    {
                        //  MessageBox.Show(tmpPostID+"--"+dataGridView1.Rows[j].Cells["tumImgPostID"].Value.ToString());
                        if (dataGridView1.Rows[j].Cells["tumImgPostID"].Value.ToString() != "")
                        {
                            int dgvPostIndex = dataGridView1.Rows[j].Cells["tumImgPostID"].Value.ToString().IndexOf("post") + 4 + 1 + Program.NumericTumPostIDLen;
                            dgvPostID = dataGridView1.Rows[j].Cells["tumImgPostID"].Value.ToString().Substring(0, tmpPostIndex);
                        }
                        else if(dataGridView1.Rows[j].Cells["tumImgPhotoSetID"].Value.ToString()!="") {
                            int dgvPostIndex = dataGridView1.Rows[j].Cells["tumImgPhotoSetID"].Value.ToString().IndexOf("post") + 4 + 1 + Program.NumericTumPostIDLen;
                            dgvPostID = dataGridView1.Rows[j].Cells["tumImgPhotoSetID"].Value.ToString().Substring(0, tmpPostIndex);
                        }
                        if (tmpPostID != dgvPostID)
                        {
                            dataGridView1.Rows[j].Visible = false;
                        }
                        else {
                            dataGridView1.Rows[j].Visible = true;
                        }
                    }


                }
            }
           

    */
           // 
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
