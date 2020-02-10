using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace TCP服务端
{
    class Program
    {
        static void Main(string[] args)
        {
            StartSercerAsync();
            Console.ReadKey();
        }

    static  void StartSercerAsync()    //异步多条信息链接
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //本机Ip:172.22.224.34 （路由器会定时更改）    万用（代表本机127.0.0.1    客户端连接时要连接指定Ip；
            //IPAddress ipAddress = new IPAddress(new byte[] { 172, 22, 224, 34 });  //用数组传入指定Ip地址
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");  //用字符串指定ip地址
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 88);
            serverSocket.Bind(ipEndPoint);             //绑定ip和端口号
            serverSocket.Listen(0);                   //开始监听端口号，排队队列的长度

            // Socket clientSocket = serverSocket.Accept();     //接收一个客户端链接, 该方法为同步方式，接收到一个客户端链接才进行下面的

            serverSocket.BeginAccept(AcceptCallBack,serverSocket);

        }

        static Message msg = new Message();       //在类中进行处理
        static byte[] dataBuffer = new byte[1024];//未经处理的直存,不能解决粘包问题

        static void AcceptCallBack(IAsyncResult ar)          //接收多个客户端链接
        {
            Socket serverSocket = ar.AsyncState as Socket;
            Socket clientSocket = serverSocket.EndAccept(ar);

            //向客户端发送一条消息
            string msgStr = "Hello client! 你好。。。";
            byte[] data = Encoding.UTF8.GetBytes(msgStr);    //必须将字符串转化为byte类型服务器端才能发送
            clientSocket.Send(data);

           // clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);//再次接收信息
            clientSocket.BeginReceive(msg.Data , msg.Length , msg.RemainSize , SocketFlags.None, ReceiveCallBack, clientSocket);

            serverSocket.BeginAccept(AcceptCallBack, serverSocket);//再次链接新客户端
        }

        static void ReceiveCallBack(IAsyncResult ar)           //异步接收多条信息
        {
            Socket clientSocket = null ;
            try
            {
                clientSocket = ar.AsyncState as Socket;
                int count = clientSocket.EndReceive(ar);
                if(count==0)//表示客户端传入空数据（客户端正常断开链接
                {
                    clientSocket.Close();
                    return;
                }
                msg.AddCount(count);
                msg.ReadMessage();
                // string msgStr = Encoding.UTF8.GetString(dataBuffer, 0, count);
                // Console.WriteLine("从客户端接收到数据:" + msgStr);
                // clientSocket.BeginReceive(dataBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);//再次接收信息
                clientSocket.BeginReceive(msg.Data, msg.Length, msg.RemainSize, SocketFlags.None, ReceiveCallBack, clientSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                if (clientSocket != null)
                    clientSocket.Close();
            }  
        }

        static void StartServerSync()        //同步的，接收了一条消息才能进行下一步
        {
            Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //本机Ip:172.22.224.34 （路由器会定时更改）    万用（代表本机127.0.0.1    客户端连接时要连接指定Ip；
            //IPAddress ipAddress = new IPAddress(new byte[] { 172, 22, 224, 34 });  //用数组传入指定Ip地址
            IPAddress ipAddress = IPAddress.Parse("172.22.224.34");  //用字符串指定ip地址
            IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, 88);
            serverSocket.Bind(ipEndPoint);             //绑定ip和端口号
            serverSocket.Listen(0);                   //开始监听端口号，排队队列的长度
            Socket clientSocket = serverSocket.Accept();     //接收一个客户端链接

            //向客户端发送一条消息
            string msg = "Hello client! 你好。。。";
            byte[] data = Encoding.UTF8.GetBytes(msg);    //必须将字符串转化为byte类型服务器端才能发送
            clientSocket.Send(data);

            //接收客户端的一条消息
            byte[] dataBuffer = new byte[1024];
            int count = clientSocket.Receive(dataBuffer);   //返回接收到dataBuffer里前count个
            string msgReceive = System.Text.Encoding.UTF8.GetString(dataBuffer, 0, count);
            Console.WriteLine(msgReceive);

            Console.ReadKey();

            clientSocket.Close();        //断开与客户端的链接
            serverSocket.Close();        //断开自身（服务器）的链接*  
        }
    }
}
