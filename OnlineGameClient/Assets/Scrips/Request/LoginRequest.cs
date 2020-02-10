using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Assets.Model;

public class LoginRequest : BaseRequest {

    private LoginPanel loginPanel;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Login;
        loginPanel = GetComponent<LoginPanel>();
        base.Awake();
    }
	
    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    public void SendRequest(string username,string password)
    {
        string data = username + "," + password;
        SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string []strs = data.Split(',');
        ReturnCode  returnCode = (ReturnCode)int.Parse(strs[0]);
        loginPanel.OnLoginResponse(returnCode);
        if (returnCode == ReturnCode.Succese)
        {
            string userName = strs[1];
            int totalCount = int.Parse(strs[2]);
            int winCount = int.Parse(strs[3]);
            UserData ud = new UserData(userName, totalCount, winCount);
            facade.SetUserData(ud);
        }
    }
}
