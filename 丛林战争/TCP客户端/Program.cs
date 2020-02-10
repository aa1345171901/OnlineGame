using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP客户端
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"),88));   //与服务器远程主机建立链接

            //接收消息
            byte[] data = new byte[1024];
            int count = clientSocket.Receive(data);   //会暂停，直到接收到信息
            string msgStr = Encoding.UTF8.GetString(data, 0, count);
            Console.WriteLine(msgStr);

            while (true)
            {
                string s = Console.ReadLine();
                if (s == "c")
                {
                    clientSocket.Close(); return;
                }
                clientSocket.Send(Message.GetBytes(s)); //分包和粘包问题：发送的信息数据量小，
                                                        //频率高便会将多个数据粘合发送给服务端，
                                                        //发送的信息过大，大于服务端的数组大小变化分包发送
            }
            //for (int i = 0; i < 100; i++)
            //{
            //    clientSocket.Send(Message.GetBytes(i.ToString ()));
            //}
            Console.ReadKey();
            clientSocket.Close();   //断开链接

        }
    }
}
