using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class RoomController:BaseController
    {

        public RoomController()
        {
            requestCode = RequestCode.Room;
        }

        public string CreateRoom(string date, Client client, Server server)
        {
            server.CreateRoom(client);
            return ((int)ReturnCode.Succese).ToString()+","+((int)RoleType.Red).ToString();
        }

        public string ListRoom(string date, Client client, Server server)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Room room  in server.GetRoomList())
            {
                if (room.IsWaitingJoin())
                {
                    sb.Append(room.GetHouseOwnerData()+"|");
                }
            }
            if (sb.Length == 0)
            {
                sb.Append("0");
            }
            else
            {
                sb.Remove(sb.Length - 1,1);
            }
            return sb.ToString();
        }

        public string JoinRoom(string data,Client client ,Server server)
        {
            int id = int.Parse(data);
            Room room = server.GetRoomById(id);
            if(room == null)
            {
                return ((int)ReturnCode.NotFound).ToString();
            }
            else if(room.IsWaitingJoin()==false)
            {
                return ((int)ReturnCode.Fail).ToString();
            }
            else
            {
                room.AddClient(client);
                string roomData = room.GetRoomData();
                room.BroadCastMsg(client, ActionCode.UpdateRoom, roomData);
                return ((int)ReturnCode.Succese).ToString() + "-" + ((int)RoleType.Blue).ToString() + "-"+roomData;
            }
        }

        public string QuitRoom(string data, Client client, Server server)
        {
            //判断退出的是否是房主
            bool isHouseOwner = client.IsHouseOwner();
            Room room = client.Room;
            if(isHouseOwner)
            {
                room.BroadCastMsg(client, ActionCode.QuitRoom, ((int)ReturnCode.Succese).ToString());
                room.Close();
                return ((int)ReturnCode.Succese).ToString();
            }
            else
            {
                try
                {
                    client.Room.RemoveClient(client);
                    room.BroadCastMsg(client, ActionCode.UpdateRoom, room.GetRoomData());
                    //Console.WriteLine(room.GetRoomData());
                    return ((int)ReturnCode.Succese).ToString();
                }
                catch (Exception e)
                {
                    return ((int)ReturnCode.Succese).ToString();
                }
            }

        }
    }
}
