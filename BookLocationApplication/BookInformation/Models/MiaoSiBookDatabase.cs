using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookInformation
{
    class MiaoSiBookDatabase
    {
        public string bookDataBaseConnectionString;
        private SqlConnection sqlConnection;
        //public string tableName = "booktable";
        private const string RFIDCodeToAccessCodeTable = "tmxxb";  //从解码后的rfid到图书索取号查询的表 确定是那个表
        //private const string RFIDCodeToAccessCodeTable = "tctmb";  //从解码后的rfid到图书索取号查询的表
        private const string AccessCodeToBookNameTable = "wxxxb";  //从图书索取号到图书名称查询的表
        public MiaoSiBookDatabase(string connectionString)
        {
            bookDataBaseConnectionString = connectionString;
        }
        private void connectionDatabase()
        {
            sqlConnection = new SqlConnection(bookDataBaseConnectionString);
            sqlConnection.Open();
        }
        private void disconnectionDatabase()
        {
            sqlConnection.Close();
        }
        public string getBookAccessCodeByDecodedRFID(string decodedBookRFID)  //从解码后的rfid数据中查到图书索取号
        {
            string accessCode = "";
            //string sqlCommandString = "select " + "索取号" + " from " + RFIDCodeToAccessCodeTable + " where 条形码=" + "'" + decodedRFID + "'";
            string sqlCommandString = "select 索取号 from " + RFIDCodeToAccessCodeTable + " where 条形码=@decodedBookRFID";
            try
            {
                connectionDatabase();
                SqlCommand readCommand = new SqlCommand(sqlCommandString, sqlConnection);
                readCommand.Parameters.Add(new SqlParameter("decodedBookRFID", decodedBookRFID));
                SqlDataReader sqlReader = readCommand.ExecuteReader();
                sqlReader.Read();
                accessCode = sqlReader["索取号"].ToString();
                disconnectionDatabase();
                return accessCode.Trim();
            }
            catch (System.InvalidOperationException)  //数据库查询失败
            {
                //throw new DataBaseQueryException();
                return "";
            }
            catch (System.Data.SqlClient.SqlException) //数据库连接失败
            {
                throw new DataBaseConnectException();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string getBookNameByAccessCode(string accessCode)
        {
            //accessCode = "A-61/7";
            string bookName = "";
            //string sqlCommandString = "select " + "正题名" + " from " + AccessCodeToBookNameTable + " where 索取号=" + "'" + accessCode + "'";
            string sqlCommandString = "select " + "正题名" + " from " + AccessCodeToBookNameTable + " where 索取号=@accessCode";

            //Console.WriteLine(accessCode + "end");
            //Console.WriteLine(sqlCommandString);
            try
            {
                connectionDatabase();
                SqlCommand readCommand = new SqlCommand(sqlCommandString, sqlConnection);
                readCommand.Parameters.Add(new SqlParameter("accessCode", accessCode));
                SqlDataReader sqlReader = readCommand.ExecuteReader();
                sqlReader.Read();
                bookName = sqlReader["正题名"].ToString();
                disconnectionDatabase();
                return bookName.Trim();
            }
            catch (System.InvalidOperationException)  //数据库查询失败
            {
                throw new DataBaseQueryException();
            }
            catch (System.Data.SqlClient.SqlException) //数据库连接失败
            {
                //throw new DataBaseConnectException();
            }
            catch (Exception e)
            {
                throw e;
            }
            return "";

        }
        public string getBookNameByDecodedRFID(string decodedRFID)
        {
            string accessCode = getBookAccessCodeByDecodedRFID(decodedRFID);
            string bookName = getBookNameByAccessCode(accessCode);
            return bookName;
        }
        /***
        public string getBookNameByScanCode(string scanCode)
        {//必须根据数据表名和表内数据格式对如下代码调整++++++++++++++++++++++++++++
            string bookName="";
            SqlConnection sqlConnection = new SqlConnection(bookDataBaseConnectionString);
            try
            {
                sqlConnection.Open();
                string sqlCommandString="select bookname from "+bookDataBaseConnectionString+" where scancode="+scanCode;
                SqlCommand readCommand = new SqlCommand(sqlCommandString,sqlConnection);
                SqlDataReader sqlReader = readCommand.ExecuteReader();
                sqlReader.Read(); //不管有多少数据，只读取第一个，会不会没有数据
                bookName=sqlReader["bookname"].ToString();
            }
            catch (Exception e)  //可能抛出sql异常或者数组访问异常
            {
                throw e;  //rethrow the old exception
            }
            finally
            {
                sqlConnection.Close();
            }
            return bookName;
        }
        public List<string> getBookNameListByScanCodeList(List<string> scanCodeList)
        {//必须根据数据表名和表内数据格式对如下代码调整+++++++++++++++++++++++++
            List<string> bookNameList = new List<string>();
            SqlConnection sqlConnection = new SqlConnection(bookDataBaseConnectionString);
            try
            {
                sqlConnection.Open();
                foreach (string oneBookScanCode in scanCodeList)
                {
                    string sqlCommandString = "select bookname from " + bookDataBaseConnectionString + " where scancode=" + oneBookScanCode;
                    SqlCommand readCommand = new SqlCommand(sqlCommandString, sqlConnection);
                    SqlDataReader sqlReader = readCommand.ExecuteReader();
                    sqlReader.Read();
                    string oneBookName = sqlReader["bookname"].ToString();
                    bookNameList.Add(oneBookName);
                }
                
            }
            catch (Exception e)  //可能抛出sql异常或者数组访问异常
            {
                throw e;  //rethrow the old exception
            }
            finally
            {
                sqlConnection.Close();
            }
            return bookNameList;
        }
         * **/
    }
}
