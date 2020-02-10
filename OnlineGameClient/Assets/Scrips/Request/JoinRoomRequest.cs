using Assets.Model;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomRequest : BaseRequest {

    private RoomListPanel roomListPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.JoinRoom;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    public void SendRequest(int id)
    {
        base.SendRequest(id.ToString());
    }

    public override void OnResponse(string data)
    {
        string []strs = data.Split('-');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        UserData ud1 = null;
        UserData ud2 = null;
        if (returnCode == ReturnCode.Succese)
        {
            facade.SetCurrentRoleType(int.Parse(strs[1]));
            string[] udStrArray = strs[2].Split('|');
            //print(data);
            ud1 = new UserData(udStrArray[0]);
            ud2 = new UserData(udStrArray[1]);
        }
        roomListPanel.OnJoinResponse(returnCode ,ud1,ud2);
    }
}
