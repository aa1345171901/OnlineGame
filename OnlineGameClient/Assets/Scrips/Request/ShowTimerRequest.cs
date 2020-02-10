using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTimerRequest : BaseRequest {

    private GamePanel gamePanel;

    public override void Awake()
    {
        actionCode = ActionCode.ShowTimer;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void OnResponse(string data)
    {
        int timer = int.Parse(data);
        gamePanel.ShowTimeSync(timer);
    }
}
