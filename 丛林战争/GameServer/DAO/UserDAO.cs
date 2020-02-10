using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using GameServer.Model;

namespace GameServer.DAO
{
    class UserDAO
    {
        /// <summary>
        /// 链接数据库，检查是否存在用户，login
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User VerifyUser(MySqlConnection conn,string username,string password)
        {
            MySqlDataReader reader =null  ;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username=@username and password = @password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int id = reader.GetInt32("id");
                    User user = new User(id, username, password);
                    return user;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在执行VerifyUser时出现异常:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return null;
        }
        
        /// <summary>
        /// 查找是否存在该用户是否进行注册,ture 为重复user
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool GetUsername(MySqlConnection conn, string username)
        {
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand cmd = new MySqlCommand("select * from user where username=@username", conn);
                cmd.Parameters.AddWithValue("username", username);
                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("在执行GetUsername时出现异常:" + e);
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return false;
        }

        /// <summary>
        /// 注册用户
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void AddUser(MySqlConnection conn, string username, string password)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand("insert into user set username=@username,password=@password", conn);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                cmd.ExecuteNonQuery();   //返回值为操作的数量，更改数据库后要更execute方法
            }
            catch (Exception e)
            {
                Console.WriteLine("在执行GetUsername时出现异常: " + e);
            }
        }

    }
}
