using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using Common;
using MySql.Data.MySqlClient;
using GameServer.Tool;
using GameServer.Model;
using GameServer.DAO;

namespace GameServer.Servers
{
    class Client
    {
        private Socket clientSocket;
        private Server server;
        private Message msg = new Message();

        private MySqlConnection mysqlConn;
        private User user;
        private Result result;
        private Room room;
        private int hp;

        private ResultDAO resultDAO=new ResultDAO();

        public MySqlConnection MySqlconn{ get { return mysqlConn; } }
        public void SetUserData(User user,Result result)
        {
            this.user = user;
            this.result = result;
        }

        public int GetId()
        {
            return user.Id;
        }

        public Room Room { set { room = value; }get { return room; } } 
        public int Hp  { set { hp = value; }get { return hp; } } 

        public bool TakeDamage(int damage)
        {
            Hp -= damage;
            Hp = Math.Max(hp, 0);
            if (Hp <= 0)
                return true;
                return false;
        }

        public bool IsDie()
        {
            return Hp <= 0;
        }

        public string  GetUserData()
        {
            return user.Id+","+user.Username + "," + result.TotalCount + "," + result.WinCount;
        }

        public Client() { }
        public Client(Socket clientSocket,Server server )
        {
            mysqlConn = ConnHelper.Connect();
            Console.WriteLine("一个客户端连接");
            this.clientSocket = clientSocket;
            this.server = server; 
        }
 
        public void Start()
        {
            if (clientSocket == null || clientSocket.Connected == false) { room.QuitRoom(this); return; }
            clientSocket.BeginReceive(msg.Data, msg.StartIndex , msg.RemainSize, SocketFlags.None, ReceiveCallBack, null);            
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                if (clientSocket == null || clientSocket.Connected == false) return;
                int count = clientSocket.EndReceive(ar);
                if (count <= 0)
                {
                    Close();
                }
                //todo
                msg.ReadMessage(count, OnProcessMessage);
                Start();
            }
            catch (Exception e)
            {
                Close();
                Console.WriteLine(e);
            }
        } 

        public void OnProcessMessage(RequestCode requestCode,ActionCode actionCode,string data)
        {
            try { server.RequestHander(requestCode, actionCode, data, this); }
            catch (Exception e) { Console.WriteLine(e); }
            
        }

        public void Send(ActionCode actionCode,string data )
        {
           byte [] bytes= Message.PackData(actionCode, data);
            try
            {
                clientSocket.Send(bytes);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }

        }

        private  void Close()
        {
            ConnHelper.CloseConnection(mysqlConn);
            if (room != null)
            {
                room.QuitRoom(this);
            }
            if (this.clientSocket !=null)
            {
                clientSocket.Close();  //断掉client链接
            }
            server.RemoveClient(this);
        }

        public bool IsHouseOwner()
        {
            if (room != null)
                return room.IsHouseOwner(this);
            else
                return false;
        }

        public void UpdateResult(bool IsVictory)
        {
            UpdateResultDB(IsVictory);
        }

        public void UpdateResultDB(bool IsVictory)
        {
            result.TotalCount++;
            if (IsVictory)
            {
                result.WinCount++;
            }
            resultDAO.UpdateOrAddResult(mysqlConn, result);
        }

        public void UpdateResultClient()
        {
            Send(ActionCode.UpdateResult, string.Format("{0},{1}", result.TotalCount, result.WinCount));
        }
    }
}
