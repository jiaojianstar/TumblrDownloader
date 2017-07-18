using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
namespace TumblrDownloader
{
    class DBOperator
    {
        private string DBFilePath = "";
        private string DBConnStr = "";
        private SQLiteConnection sLite3Conn = null;
        private SQLiteCommand sLite3CMD = null;
        public DBOperator()
        {
            if (Program.DBFilePath == "default")
            {
                DBFilePath = Environment.CurrentDirectory + "\\" + Program.DBFileName;
                DBConnStr = "Data Source=" + DBFilePath + ";Version=3;";
                Console.WriteLine(DBFilePath);
                sLite3Conn = new SQLiteConnection(DBConnStr);
                sLite3Conn.Open();
                sLite3CMD = sLite3Conn.CreateCommand();
            }
        }
        public void AddImgInfoToDB(string cmd)
        {

            try
            {


                sLite3CMD.CommandText = cmd;
                sLite3CMD.ExecuteNonQuery();

            }
            catch (Exception e)
            {

            }

        }
        public int GetImgIndex()
        {
            int currImgIndex = 1;
            sLite3CMD.CommandText = "select count(*) from TumblrImgsDetails";
            try
            {
                object tmpObj = sLite3CMD.ExecuteScalar();
                if (tmpObj != null)
                {
                    currImgIndex = int.Parse(tmpObj.ToString()) + 1;

                }




                return currImgIndex;
            }
            catch (Exception e)
            {
                return -1;
            }
            

        }
        public void UpdateTumblrResourceItem(string resIndex,string resSize,string resDNTime)
        {
            sLite3CMD.CommandText = "update TumblrImgsDetails SET tumImgSize='"+resSize
                +"',tumImgDownTime='"+resDNTime
                + "' WHERE tumImgIndex='"+resIndex
                +"' ";
            try
            {
                sLite3CMD.ExecuteNonQuery();

            }
            catch (Exception e)
            {
            }


        }
        public void Close()
        {
            try
            {
                if (sLite3CMD != null)
                {
                    sLite3CMD = null;
                }
                if (sLite3Conn.State == System.Data.ConnectionState.Open)
                {
                    sLite3Conn.Close();
                }
            }
            catch (Exception e)
            {
            }
          
           
        }
        public DataTable GetTestData()
        {
            DataTable dt = new DataTable();
            sLite3CMD.CommandText = "SELECT * FROM TumblrImgsDetails limit 6000,50";



            try
            {
                SQLiteDataAdapter sda = new SQLiteDataAdapter(sLite3CMD);
                sda.Fill(dt);

            }
            catch (Exception e)
            {
            }
            return dt;


        }
        
        
    }
}
