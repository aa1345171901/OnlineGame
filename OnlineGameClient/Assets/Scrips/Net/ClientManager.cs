using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using Common;
using System;

public class ClientManager : BaseManager {

    private const string IP = "127.0.0.1";
    private const int PORT = 6688;

    private Socket clientSocket;

    private Message msg=new Message();

    public ClientManager(GameFacade facade) : base(facade) { }
    /// <summary>
    /// 对client manager进行初始化
    /// </summary>
    public override void OnInit()
    {
        base.OnInit();

        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
        try
        {
            clientSocket.Connect(IP, PORT);
            Start();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("无法链接到服务器端，请检查您的网络！！" + e);
        }
    }

    private void Start()
    {
        clientSocket.BeginReceive(msg.Data, msg.StartIndex , msg.RemainSize,SocketFlags.None, ReceiveCallBack, null);
    }
    /// <summary>
    /// 接收回调
    /// </summary>
    /// <param name="ar"></param>
    private void ReceiveCallBack(IAsyncResult ar)
    {
        try
        {
            if (clientSocket == null || clientSocket.Connected == false)
                return;
            int count = clientSocket.EndReceive(ar);

            msg.ReadMessage(count,OnProcessCallBack);
            Start();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    private void OnProcessCallBack(ActionCode actionCode,string data)
    {
        facade.HandleResponse(actionCode, data);
    }
/// <summary>
/// 发送请求
/// </summary>
/// <param name="requestCode"></param>
/// <param name="actionCode"></param>
/// <param name="data"></param>
    public void SendRequest(RequestCode requestCode,ActionCode actionCode,string data)
    {
       byte[] bytes=Message.PackData(requestCode, actionCode, data);
        clientSocket.Send(bytes);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        try
        {
            clientSocket.Close();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("无法断开与服务器的链接！！" + e);
        }
    }
}
