using Assets.Model;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRoomRequest : BaseRequest {

    private RoomPanel roomPanel;

    public override void Awake()
    {
        requestCode = RequestCode.Room;
        actionCode = ActionCode.UpdateRoom;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
       // print(data);
        UserData ud1 = null;
        UserData ud2 = null;
        string[] udStrArray = data.Split('|');
        if (udStrArray.Length > 1)
        {
            ud1 = new UserData(udStrArray[0]);
            ud2 = new UserData(udStrArray[1]);
            roomPanel.SetAllPlayerResSync(ud1, ud2);
        }
        else
        {
            ud1 = new UserData(udStrArray[0]);
            roomPanel.SetAllPlayerResSync(ud1, ud2);
        }
    }
}
