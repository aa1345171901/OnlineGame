using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitBattleRequest : BaseRequest {

    private bool isQuitBattle = false;
    private GamePanel gamePanel;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.QuitBattle;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    public override void SendRequest()
    {
        base.SendRequest("4r");
    }

    private void Update()
    {
        if (isQuitBattle)
        {
            gamePanel.OnExitResponse();
            isQuitBattle=false;
        }
    }

    public override void OnResponse(string data)
    {
        isQuitBattle = true;
    }
}
