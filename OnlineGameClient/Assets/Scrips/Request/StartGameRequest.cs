using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameRequest : BaseRequest {

    private RoomPanel roomPanel;
    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.StartGame;
        roomPanel = GetComponent<RoomPanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("R");
        //print(1);
    }

    public override void OnResponse(string data)
    {
        ReturnCode returnCode = (ReturnCode)int.Parse(data);
        //print(1+ data);
        roomPanel.OnStartResponse(returnCode);
    }
}
