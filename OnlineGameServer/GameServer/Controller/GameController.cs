﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using GameServer.Servers;

namespace GameServer.Controller
{
    class GameController : BaseController
    {
        public GameController()
        {
            requestCode = RequestCode.Game;
        }

        public string StartGame(string data, Client client, Server server)
        {
            if (client.IsHouseOwner())
            {
                Room room = client.Room;
                //if (room.IsWaitingBattle())
                //{
                    room.BroadCastMsg(client, ActionCode.StartGame, ((int)ReturnCode.Succese).ToString());
                    room.StartTimer();
                    return ((int)ReturnCode.Succese).ToString();
                //}
                //else
                //    return "";
            }
            else
            {
                return ((int)ReturnCode.Fail).ToString();
            }
        }

        public string Move(string data, Client client, Server server)
        {
            Room room = client.Room;
            if(room!=null)
            room.BroadCastMsg(client, ActionCode.Move, data);
            return null;
        }

        public string Shoot(string data, Client client, Server server)
        {
            Room room = client.Room;
            if (room != null)
                room.BroadCastMsg(client, ActionCode.Shoot, data);
            return null;
        }

        public string Attack(string data, Client client, Server server)
        {
            int damage = int.Parse(data);
            Room room = client.Room;
            if (room != null)
                room.TakeDamage(damage,client);
            return null;
        }

        public string QuitBattle(string data, Client client, Server server)
        {
            Room room = client.Room;
            room.TakeDamage(1000, client);
            if (room != null)
            {
                room.BroadCastMsg(null, ActionCode.QuitBattle, "r");
                room.Close();
            }
            return null;
        }

    }
}
