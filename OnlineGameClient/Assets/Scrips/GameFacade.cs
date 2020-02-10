using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Assets.Model;

public class GameFacade : MonoBehaviour {

    private static GameFacade _instance;
    public static GameFacade Instance { get
        {
            //if (_instance == null)
            //{
            //    _instance = GameObject.Find("GameFacade").GetComponent<GameFacade>();
            //}
            return _instance;
        } }

    private UIManager uiMng;
    private AudioManager audioMng;
    private PlayerManager playerMng;
    private RequestManager requestMng;
    private CameraManager cameraMng;
    private ClientManager clientMng;

    private bool isEntering=false;

	// Use this for initialization
	void Start () {
        InitManager();
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject ); return;
        }
        _instance = this;
    }
    // Update is called once per frame
    void Update () {
        UpdateManager();
        if (isEntering)
        {
            EnterPlaying();
            isEntering = false;
        }
	}
    /// <summary>
    /// 初始化各个Manager
    /// </summary>
    private void InitManager()
    {
        uiMng = new UIManager(this);
        audioMng = new AudioManager(this);
        playerMng = new PlayerManager(this);
        requestMng = new RequestManager(this);
        cameraMng = new CameraManager(this);
        clientMng = new ClientManager(this);

        uiMng.OnInit();
        audioMng.OnInit();
        playerMng.OnInit();
        requestMng.OnInit();
        cameraMng.OnInit();
        clientMng.OnInit();
    }

    /// <summary>
    /// 更新manager
    /// </summary>
    private void UpdateManager()
    {
        uiMng.Update();
        audioMng.Update();
        playerMng.Update();
        requestMng.Update();
        cameraMng.Update();
        clientMng.Update();
    }

    /// <summary>
    /// 销毁各个manager
    /// </summary>
    private void DestroyManager()
    {
        uiMng.OnDestroy();
        audioMng.OnDestroy();
        playerMng.OnDestroy();
        requestMng.OnDestroy();
        cameraMng.OnDestroy();
        clientMng.OnDestroy();
    }
    /// <summary>
    /// 游戏物体销毁时调用
    /// </summary>
    private void OnDestroy()
    {
        DestroyManager();
    }
    /// <summary>
    /// 添加请求Action至服务器
    /// </summary>
    /// <param name="actiontCode"></param>
    /// <param name="baseRequest"></param>
    public void AddRequest(ActionCode actiontCode,BaseRequest baseRequest)
    {
        requestMng.AddRequest(actiontCode, baseRequest);
    }
    /// <summary>
    /// 去除请求
    /// </summary>
    /// <param name="actiontCode"></param>
    public void RemoveRequest(ActionCode actiontCode)
    {
        requestMng.RemoveRequest(actiontCode);
    }
    /// <summary>
    /// 对请求做出响应
    /// </summary>
    /// <param name="actiontCode"></param>
    /// <param name="data"></param>
    public void HandleResponse(ActionCode actiontCode, string data)
    {
        requestMng.HandleResponse(actiontCode, data);
    }
    /// <summary>
    /// 向服务器发送请求
    /// </summary>
    /// <param name="requestCode"></param>
    /// <param name="actionCode"></param>
    /// <param name="data"></param>
    public void SendRequest(RequestCode requestCode, ActionCode actionCode, string data)
    {
        clientMng.SendRequest(requestCode, actionCode, data);
    }

    /// <summary>
    /// 显示提示信息
    /// </summary>
    /// <param name="msg"></param>
    public void ShowMessage(string msg)
    {
        uiMng.ShowMessage(msg);
    }

    /// <summary>
    /// 通过Audio manager播放声音
    /// </summary>
    /// <param name="soundName"></param>
    public void PlayBgSound(string soundName)
    {
        audioMng.PlayBgSound( soundName);
    }

    public void PlayNormalSound(string soundName)
    {
        audioMng.PlayNormalSound(soundName);
    }

    /// <summary>
    /// 通过调用来设置player的userdata
    /// </summary>
    /// <param name="userData"></param>
    public void SetUserData(UserData userData)
    {
        playerMng.UserData = userData;
    }

    /// <summary>
    /// 获得userdata
    /// </summary>
    /// <returns></returns>
    public UserData GetUSerData()
    {
        return playerMng.UserData;
    }

    /// <summary>
    /// 设置角色类型
    /// </summary>
    /// <param name="rt"></param>
    public void SetCurrentRoleType(int rt)
    {
        playerMng.SetCurrentRoleType(rt);
    }

    /// <summary>
    /// 获取角色游戏物体
    /// </summary>
    /// <returns></returns>
    public GameObject GetCurrentGameObject()
    {
        return playerMng.GetCurrentGameObject();
    }

    public void EnterPlayingSync()
    {
        isEntering = true;
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    public void EnterPlaying()
    {
        playerMng.SpawnRoles();
        cameraMng.FollowRole(GetCurrentGameObject().transform);
    }

    /// <summary>
    /// 添加角色控制脚本
    /// </summary>
    public void AddControlScript()
    {
        playerMng.AddControlScript();
        playerMng.CreateSyncRequest();
    }

    /// <summary>
    /// 攻击减血
    /// </summary>
    /// <param name="damage"></param>
    public void SendAttack(int damage)
    {
        playerMng.SendAttack(damage);
    }

    /// <summary>
    /// 判断游戏结束
    /// </summary>
    public void GameOver()
    {
        cameraMng.WalkThrouthScene();
        playerMng.GameOver();
    }

    /// <summary>
    /// 游戏结束后更新数据
    /// </summary>
    /// <param name="totalCount"></param>
    /// <param name="winCount"></param>
    public void UpdateUserData(int totalCount, int winCount)
    {
        playerMng.UpdateUserData(totalCount,winCount);
    }
}
