using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPlayRequest : BaseRequest {

    private bool IsAddControlScript = false;

    public override void Awake()
    {
        actionCode = Common.ActionCode.StartPlay;
        base.Awake();
    }

    private void Update()
    {
        if (IsAddControlScript)
        {
            facade.AddControlScript();
            IsAddControlScript = false;
        }
    }

    public override void OnResponse(string data)
    {
        IsAddControlScript = true;
    }
}
