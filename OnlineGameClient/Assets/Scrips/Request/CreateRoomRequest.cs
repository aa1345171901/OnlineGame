using Assets.Model;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomRequest : BaseRequest {

    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.CreateRoom;
        base.Awake();
    }

    public void SetPanel(BasePanel panel)
    {
        roomPanel = panel as RoomPanel;
    }

    public override void SendRequest()
    {
        base.SendRequest("r");
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split(',');
        ReturnCode returnCode = (ReturnCode)int.Parse(strs[0]);
        facade.SetCurrentRoleType(int.Parse(strs[1]));
        if (returnCode == ReturnCode.Succese)
        {
            roomPanel.SetLocalPlayerResSync();
        }
    }
}
