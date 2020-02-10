using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class RequestManager : BaseManager {

    Dictionary<ActionCode , BaseRequest> requestDict = new Dictionary<ActionCode, BaseRequest>();

    public RequestManager(GameFacade facade) : base(facade) { }

    public void AddRequest(ActionCode actionCode ,BaseRequest baseRequest)
    {
        requestDict.Add(actionCode, baseRequest);
    }

    public void RemoveRequest(ActionCode actionCode)
    {
        requestDict.Remove(actionCode);
    }

    public void HandleResponse(ActionCode actionCode, string data)
    {
       BaseRequest request= requestDict.TryGet<ActionCode, BaseRequest>(actionCode);
       // Debug.Log(actionCode);
        if (request == null)
        {
            Debug.LogWarning("无法得到ActionCode[" + actionCode + "]对应的类");return;
        }
        request.OnResponse(data);
    }
}
