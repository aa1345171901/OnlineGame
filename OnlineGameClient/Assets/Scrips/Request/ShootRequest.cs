using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootRequest : BaseRequest {

    public PlayerManager playerManager;
    private bool isShoot=false;

    private RoleType roleType;
    private Vector3 pos;
    private Vector3 rotation;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Shoot;
        base.Awake();
    }

    private void Update()
    {
        if (isShoot)
        {
            playerManager.RemoteShoot(roleType, pos, rotation);
            isShoot = false;
        }
    }

    public void SendRequest(RoleType roleType,Vector3 pos,Vector3 rotation)
    {
        string data = string.Format("{0}|{1},{2},{3}|{4},{5},{6}", (int)roleType,pos.x,pos.y,pos.z, rotation.x, rotation.y, rotation.z);
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        roleType = (RoleType)int.Parse(strs[0]);
        pos = UnityTools.Parse(strs[1]);
        rotation = UnityTools.Parse(strs[2]);
        isShoot = true;
    }
}
