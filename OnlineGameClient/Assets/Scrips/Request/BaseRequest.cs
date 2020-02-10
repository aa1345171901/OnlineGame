using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class BaseRequest : MonoBehaviour {

    protected RequestCode requestCode = RequestCode.None;
    protected ActionCode actionCode = ActionCode.None;
    protected GameFacade _facade;

    protected GameFacade facade
    {
        get
        {
            if (_facade == null)
                _facade = GameFacade.Instance;
            return _facade;
        }
    }

    public virtual void Awake()
    {
        facade.AddRequest(actionCode, this);
    }
    /// <summary>
    /// 向服务器发送请求
    /// </summary>
    /// <param name="data"></param>
    public void SendRequest(string data)
    {
        facade.SendRequest(requestCode, actionCode, data);
    }

    public virtual void SendRequest() { }
    public virtual void OnResponse(string data) { }

    /// <summary>
    /// 游戏物体消除时，移除请求
    /// </summary>
    public void OnDestroy()
    {
        if(facade!=null)
            facade.RemoveRequest(actionCode);
    }

}
