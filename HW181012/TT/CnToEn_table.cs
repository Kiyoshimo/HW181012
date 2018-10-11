using System;
using System.Data.SqlClient;
using OpenData;
using System.Xml.Linq;
using System.Linq;
using System.Collections.Generic;
using XMLanalysis.Shared;


namespace XMLanalysis
{
    class CnToEn_table : MGenericsDB<CnToEn>
    {
        //连接资料库
        SqlConnection connection    
            = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=" 
                + SharedDB.GetDataPath() 
                + @"mDB.mdf" 
                + ";Integrated Security=True");

        private static int count = 0;//條目插入計數器=ID

        //取值函数
        public static string getValue(XElement node, string propertyName)
        {
            return node.Element(propertyName)?.Value.Trim();
        }

 
        //删除函数
        public void DeleteData(string deleteColumn, string deleteName)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText 
                = string.Format($"DELETE CnToEn WHERE {deleteColumn}= N'{deleteName}' ");
            cmd.ExecuteNonQuery();//返回受影响的函数！
            connection.Close();
        }

        //插入各條數據
        public void InsertData(CnToEn item)
        {
            count++;//id
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText 
                = string.Format($"insert into CnToEn (Id,Col1,Col2,Col3) " 
                    +$"values ('{count}',N'{item.Postcode}',N'{item.Loc}',N'{item.Name}')");
            cmd.ExecuteNonQuery();
            connection.Close();

        }

        //查询
        public List<CnToEn> QueryData(string searchColumn, string searchName)
        {
            connection.Open();//打开数据库
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = string.Format($"SELECT * FROM CnToEn WHERE {searchColumn}= N'{searchName}' ");

            SqlDataReader reader = cmd.ExecuteReader();

            var mFarm = new List<CnToEn>();
            try
            {
                while (reader.Read())
                {
                    var item = new CnToEn
                    {
                        Postcode = reader[1].ToString(),
                        Loc = reader[2].ToString(),
                        Name = reader[3].ToString(),
                        
                        
                    };
                    mFarm.Add(item);


                }
            }
            finally
            {
                reader.Close();

            }
            connection.Close();
            return mFarm;
        }

        //Show
        public void ShowData(List<CnToEn> list)
        {
            list.ForEach(item => {
                Console.WriteLine(string.Format($"邮编: {item.Postcode} 英文名: {item.Name} 地址: {item.Loc}"));
            });

        }

        //上传函数
        public void UpdateData(int updateID, CnToEn item)
        {
            connection.Open();
            SqlCommand cmd = connection.CreateCommand();
            cmd.CommandText = string.Format($"UPDATE CnToEn SET Col1 = N'{item.Postcode}', Col2 = N'{item.Loc}', Col3 = N'{item.Name}' WHERE Id = {updateID} ");
            cmd.ExecuteNonQuery();
            connection.Close();

          
        }


        //读取XML
        public List<CnToEn> Xml_Load()
        {

            XDocument docNew = XDocument.Load(@".\..\..\OpenData\CnToEn.xml");//打開xml位置

            IEnumerable<XElement> nodes = docNew.Element("table").Elements("row");
            var nodeList = new List<CnToEn>();
            nodeList = nodes
                .Select(node => {
                    var item = new CnToEn();
                    item.Postcode = getValue(node, "Col1");
                    item.Loc = getValue(node, "Col2");
                    item.Name = getValue(node, "Col3");
                    return item;
                }).ToList();

            return nodeList;

            
        }
    }
}
