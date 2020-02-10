using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQL数据库操作
{
    class Program
    {
        static void Main(string[] args)
        {
            string connStr = "DataBase=test;Data Source=127.0.0.1;port=3306;user id=root;password=root;";
            MySqlConnection conn = new MySqlConnection(connStr);

            conn.Open();


            #region 查询 
            //MySqlCommand cmd = new MySqlCommand("select * from user",conn);

            //MySqlDataReader reader= cmd.ExecuteReader();

            //while (reader.Read())
            //{
            //    string username = reader.GetString("username");
            //    string password = reader.GetString("password");
            //    Console.WriteLine(username + ": " + password);
            //}
            //reader.Close();
            #endregion

            #region 插入
            //string username = "tmx";
            //string password = "123";
            ////MySqlCommand cmd = new MySqlCommand("insert into user set username='" + username + "',password='" + password + "'",conn );  //存在玩家恶意sql语句注入问题
            //MySqlCommand cmd = new MySqlCommand("insert into user set username=@un,password=@pwd",conn );
            //cmd.Parameters.AddWithValue("un", username);
            //cmd.Parameters.AddWithValue("pwd",password);

            //cmd.ExecuteNonQuery();
            #endregion

            #region 删除
            //MySqlCommand cmd = new MySqlCommand("delete from user where iduser=@id", conn);
            //cmd.Parameters.AddWithValue("id", 1);

            //cmd.ExecuteNonQuery();
            #endregion

            #region 修改
            MySqlCommand cmd = new MySqlCommand("update user set password = @pwd where username='tmx'", conn);
            cmd.Parameters.AddWithValue("pwd", "tmx");

            cmd.ExecuteNonQuery();
            #endregion
            conn.Close();


            Console.ReadKey();

        }
    }
}
