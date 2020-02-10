using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using GameServer.Controller;
using Common;

namespace GameServer.Servers
{
    class Server
    {
        private IPEndPoint ipEndPoint;
        private Socket ServerSocket;
        private List<Client> clients=new List<Client>();
        private List<Room> roomList = new List<Room>();
        private ControllerManager controllerManager;

        public Server() { }
        public Server(string ipAddress,int port)
        {
            controllerManager = new ControllerManager(this);
            SetIpAndPort(ipAddress, port);
        }

        /// <summary>
        /// 设置IP
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void SetIpAndPort(string ipAddress, int port)
        {
            ipEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);
        }

        /// <summary>
        /// 在这里绑定ip
        /// </summary>
        public void Start()
        {
            ServerSocket = new Socket(AddressFamily.InterNetwork,SocketType.Stream ,ProtocolType.IP );
            ServerSocket.Bind(ipEndPoint);
            ServerSocket.Listen(0);
            ServerSocket.BeginAccept(AcceptCallBack,null );
        }

        /// <summary>
        /// 异步通信，接收回调
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = ServerSocket.EndAccept(ar);
            Client client = new Client(clientSocket, this);
            client.Start();
            clients.Add(client);
            ServerSocket.BeginAccept(AcceptCallBack, null);
        }

        public void RemoveClient(Client client)
        {
            lock(clients )
            clients.Remove(client);
        }

        public void SendResponse(ActionCode actionCode,Client client ,string data)//给客户端做出响应
        {
            //Console.WriteLine(actionCode);
            if(clients.Contains(client))
            client.Send(actionCode, data);
        }

        public void RequestHander(RequestCode requestCode, ActionCode actionCode, string data, Client client)
        {
            controllerManager.RequestHander(requestCode, actionCode, data, client);
        }

        public void CreateRoom(Client client)
        {
            Room room = new Room(this);
            room.AddClient(client);
            roomList.Add(room);
        }

        public void RemoveRoom(Room room)
        {
            if (roomList != null && room != null)
            {
                roomList.Remove(room);
            }
        }

        public List<Room> GetRoomList()
        {
            return roomList;
        }

        public Room GetRoomById(int id)
        {
            foreach(Room room in roomList)
            {
                if (room.GetId() == id)
                {
                    return room;
                }
            }
            return null;
        }
    }
}
