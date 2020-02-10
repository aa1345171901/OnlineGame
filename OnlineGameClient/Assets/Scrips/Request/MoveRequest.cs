using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRequest : BaseRequest {

    private Transform localPlayerTransform;
    private PlayerMove localPlayerMove;
    private int syncRate = 20;

    private Dictionary<RoleData, Transform> remotePlayerTransformList = new Dictionary<RoleData, Transform>();
    private Animator remotePlayerAnim;

    private bool isSyncRemotePlayer=false;

    private Vector3 pos;
    private Vector3 rotation;
    private float forward;
    private bool OnGround=false;

    public override void Awake()
    {
        requestCode = RequestCode.Game;
        actionCode = ActionCode.Move;
        base.Awake();
    }

    private void Start()
    {
        InvokeRepeating("SyncLocalPlayer", 3f, 1f / syncRate);
    }

    private void FixedUpdate()
    {
        if (isSyncRemotePlayer)
        {
            SyncRemotePlayer();
            isSyncRemotePlayer = false;
        }
    }

    public MoveRequest SetLocalPlayer(Transform localPlayerTransform,PlayerMove playerMove)
    {
        this.localPlayerTransform = localPlayerTransform;
        this.localPlayerMove = playerMove;
        return this;
    }

    public MoveRequest SetRemotePlayer(Dictionary<RoleData,Transform> remotePlayerTransformList)
    {
        this.remotePlayerTransformList = remotePlayerTransformList;
        //this.remotePlayerAnim = remotePlayerTransform.GetComponent<Animator>();
        return this;
    }

    private void SyncLocalPlayer()
    {
        SendRequest(localPlayerTransform.position.x, localPlayerTransform.position.y, localPlayerTransform.position.z,
            localPlayerTransform.eulerAngles.x, localPlayerTransform.eulerAngles.y, localPlayerTransform.eulerAngles.z,
            localPlayerMove.forward, localPlayerMove.OnGround);
    }

    private void SyncRemotePlayer()
    {
        foreach(Transform item in remotePlayerTransformList.Values)
        {
            item.position = pos;
            item.eulerAngles = rotation;
            item.GetComponent<Animator>().SetFloat("Forward", forward);
            item.GetComponent<Animator>().SetBool("OnGround", OnGround);
        }

    }

    public void SendRequest(float x,float y,float z,float rotationX, float rotationY, float rotationZ,float forward,bool OnGround)
    {
        string data = string.Format("{0},{1},{2}|{3},{4},{5}|{6}|{7}", x, y, z, rotationX, rotationY, rotationZ, forward, OnGround.ToString());
        base.SendRequest(data);
    }

    public override void OnResponse(string data)
    {
        string[] strs = data.Split('|');
        pos = UnityTools.Parse(strs[0]);
        rotation= UnityTools.Parse(strs[1]);
        forward = float.Parse(strs[2]);
        OnGround =bool.Parse(strs[3]);
        isSyncRemotePlayer = true;
    }
}
