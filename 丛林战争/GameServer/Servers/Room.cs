using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameServer.Servers
{
    enum RoomState
    {
        WaitingJoin,
        WaitingBattle,
        Battle,
        End
    }
    class Room
    {
        private List<Client> clientRoom = new List<Client>();
        private RoomState state = RoomState.WaitingJoin;

        private Server server;

        private const int MAX_HP = 200;

        public Room(Server server)
        {
            this.server = server;
        }

        public bool IsWaitingJoin()
        {
            return state == RoomState.WaitingJoin;
        }

        public bool IsWaitingBattle()
        {
            return state == RoomState.WaitingBattle;
        }

        public void AddClient(Client client)
        {
            client.Hp = MAX_HP;
            clientRoom.Add(client);
            client.Room = this;
            if(clientRoom.Count>=2)
            {
                state = RoomState.WaitingBattle;
            }
        }

        public void RemoveClient(Client client)
        {
            client.Room = null;
            clientRoom.Remove(client);
            if (clientRoom.Count >= 2)
            {
                state = RoomState.WaitingBattle;
            }
            else
            {
                state = RoomState.WaitingJoin;
            }
        }

        public string GetHouseOwnerData()
        {
            return clientRoom[0].GetUserData();
        }

        public int GetId()
        {
            if(clientRoom.Count>0)
            {
                return clientRoom[0].GetId();
            }
            return -1;
        }

        public String GetRoomData()
        {
            StringBuilder sb = new StringBuilder();
            foreach(Client client in clientRoom)
            {
                sb.Append(client.GetUserData() + "|");
            }
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public void BroadCastMsg(Client excludeClient,ActionCode actionCode,string data)
        {
            foreach (var client in clientRoom)
            {
                if(client != excludeClient)
                {
                    server.SendResponse(actionCode, client, data);
                }
            }
        }

        public bool IsHouseOwner(Client client)
        {
            return client == clientRoom[0];
        }

        public void Close()
        {
            foreach(Client client in clientRoom)
            {
                client.Room = null;
            }
            server.RemoveRoom(this);
        }

        public void QuitRoom(Client client)
        {
            if (client == clientRoom[0])
                Close();
            else
                clientRoom.Remove(client);
        }

        public void StartTimer()
        {
            new Thread(RunTimer).Start();
        }

        private void RunTimer()
        {
            Thread.Sleep(1000);
            for (int i=3;i>0; i--)
            {
                BroadCastMsg(null, ActionCode.ShowTimer, i.ToString());
                Thread.Sleep(1000);
            }
            BroadCastMsg(null, ActionCode.StartPlay,"r");
        }

        public void TakeDamage(int damage,Client client)
        {
            bool IsOver = false;
            int count = 0;
            Console.WriteLine(clientRoom.Count);
            foreach (Client cli in clientRoom)
            {
                if (cli == client)
                {
                    cli.TakeDamage(damage);
                }
                if (cli.IsDie())
                {
                    count++;
                }
            }
            if (count >= clientRoom.Count - 1)
            {
                IsOver = true;
            }
            if (IsOver == false) return;
            foreach (Client cli in clientRoom)
            {
                if (cli.IsDie())
                {
                    if(clientRoom.Count!=1)
                        cli.UpdateResult(false);
                    cli.Send(ActionCode.GameOver, ((int)ReturnCode.Fail).ToString());
                }
                else
                {
                    if (clientRoom.Count != 1)
                        cli.UpdateResult(true);
                    cli.Send(ActionCode.GameOver, ((int)ReturnCode.Succese).ToString());
                }
            }
            Close();
        }
    }
}
