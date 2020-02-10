using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateResultRequest : BaseRequest {

    private RoomListPanel roomListPanel;
    private bool isUpdateResult = false;
    int totalCount;
    int winCount;

    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.UpdateResult;
        roomListPanel = GetComponent<RoomListPanel>();
        base.Awake();
    }

    private void Update()
    {
        if (isUpdateResult)
        {
            roomListPanel.OnUpdateResultResponse(totalCount, winCount);
            isUpdateResult = false;
        }
    }

    public override void OnResponse(string data)
    {
        string []strs = data.Split(',');
        totalCount = int.Parse(strs[0]);
        winCount = int.Parse(strs[1]);
        isUpdateResult = true;
    }
}
