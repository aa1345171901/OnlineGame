using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverRequest : BaseRequest {

    private GamePanel gamePanel;

    private bool IsGameOver = false;

    private ReturnCode returnCode;


    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.GameOver;
        gamePanel = GetComponent<GamePanel>();
        base.Awake();
    }

    private void Update()
    {
        if (IsGameOver)
        {
            gamePanel.OnGameOverResponse(returnCode);
            IsGameOver = false;
        }
    }

    public override void OnResponse(string data)
    {
        //print(data);
        returnCode = (ReturnCode)int.Parse(data);
        IsGameOver = true;
    }

}
