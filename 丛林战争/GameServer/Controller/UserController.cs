using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;
using GameServer.DAO;
using GameServer.Model;

namespace GameServer.Controller
{
    class UserController:BaseController
    {

        private UserDAO userDAO = new UserDAO();
        private ResultDAO resultDAO = new ResultDAO();

        public UserController()
        {
            requestCode = RequestCode.User;
        }

        public string  Login(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            User user = userDAO.VerifyUser(client.MySqlconn, strs[0], strs[1]);
           // Console.WriteLine(strs[0] + strs[1]);
            if(user == null)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                Result result = resultDAO.GetResultByUserId(client.MySqlconn,user.Id);
                client.SetUserData(user, result);
                return   string.Format("{0},{1},{2},{3}", ((int)ReturnCode.Succese).ToString(), user.Username, result.TotalCount, result.WinCount);
            }
        }

        public string Register(string data, Client client, Server server)
        {
            string[] strs = data.Split(',');
            string username = strs[0];
            string password = strs[1];
            bool res = userDAO.GetUsername(client.MySqlconn,username);
            if (res)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            userDAO.AddUser(client.MySqlconn, username, password);
            return ((int)ReturnCode.Succese).ToString();
        }
    }
}
