using Assets.Model;
using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomListPanel : BasePanel {

    private RectTransform battleRes;
    private RectTransform roomList;

    private VerticalLayoutGroup roomLayout;
    private GameObject roomItemPrefab;

    private ListRoomRequest listRoomRequest;
    private CreateRoomRequest createRoomRequest;
    private JoinRoomRequest joinRoomReques;
    private UserData ud1;
    private UserData ud2;

    private List<UserData> udList = new List<UserData>();


    private void Start()
    {
        battleRes = transform.Find("BattleRes").GetComponent<RectTransform>();
        roomList = transform.Find("RoomList").GetComponent<RectTransform>();

        roomLayout = transform.Find("RoomList/ScrollRect/Layout").GetComponent<VerticalLayoutGroup>();
        roomItemPrefab = Resources.Load("UIItem/RoomItem") as GameObject;

        transform.Find("RoomList/CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
        transform.Find("RoomList/CreateRoom").GetComponent<Button>().onClick.AddListener(OnCreteRoomClick);
        transform.Find("RoomList/RefreshButton").GetComponent<Button>().onClick.AddListener(OnRefreshClick);

        createRoomRequest = GetComponent<CreateRoomRequest>();
        joinRoomReques = GetComponent<JoinRoomRequest>();
        listRoomRequest = GetComponent<ListRoomRequest>();
        EnterAnim();
       
    }

    private void Update()
    {
        if (udList != null)
        {
            LoadRoomItem(udList);
            udList = null;
        }
        if (ud1 != null && ud2 != null)
        {
            BasePanel panel = uiMng.PushPanel(UIPanelType.Room);
            (panel as RoomPanel).SetAllPlayerResSync(ud1, ud2);
            ud1 = null;ud2 = null;
        }
    }

    private void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }

    private void OnRefreshClick()
    {
        listRoomRequest.SendRequest();
    }

    private void OnCreteRoomClick()
    {
        PlayClickSound();
        BasePanel panel= uiMng.PushPanel(UIPanelType.Room);
        createRoomRequest.SetPanel(panel);
        createRoomRequest.SendRequest();
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        if(battleRes!=null)
        EnterAnim();
        SetBattleRes();
        if (listRoomRequest == null)
            listRoomRequest = GetComponent<ListRoomRequest>();
        listRoomRequest.SendRequest();

    }

    public override void OnExit()
    {
        HideAnim();
    }

    /// <summary>
    /// 在加载其他面板时暂停
    /// </summary>
    public override void OnPause()
    {
        HideAnim();
    }

    public override void OnResume()
    {
        EnterAnim();
    }

    /// <summary>
    /// 进入动画
    /// </summary>
    private void EnterAnim()
    {
        gameObject.SetActive(true);
        battleRes.localPosition = new Vector3(-1000,0);
        battleRes.DOLocalMoveX(-236, 0.5f);

        roomList.localPosition = new Vector3(1000, 0);
        roomList.DOLocalMoveX(96, 0.5f);
    }

    /// <summary>
    /// 退出动画
    /// </summary>
    private void HideAnim()
    {
        battleRes.DOLocalMoveX(-1000, 0.5f);

        roomList.DOLocalMoveX(1000, 0.5f).OnComplete(()=>gameObject.SetActive (false));
    }

    /// <summary>
    /// 设置参数
    /// </summary>
    private void SetBattleRes()
    {
        UserData ud = facade.GetUSerData();
        transform.Find("BattleRes/UserName").GetComponent<Text>().text = ud.userName;
        transform.Find("BattleRes/TotalCount").GetComponent<Text>().text ="总场数："+  ud.totalCount.ToString();
        transform.Find("BattleRes/WinCount").GetComponent<Text>().text = "胜利场数："+ud.winCount.ToString();
    }

    public void OnUpdateResultResponse(int totalCount,int winCount)
    {
        facade.UpdateUserData(totalCount, winCount);
        SetBattleRes();
    }


    public void LoadRoomItemSync(List<UserData> udList)
    {
        this.udList = udList;
    }

    /// <summary>
    /// 加载Item
    /// </summary>
    /// <param name="count"></param>
    private void LoadRoomItem(List<UserData> udList)
    {
        RoomItem[] riArray = roomLayout.GetComponentsInChildren<RoomItem>();
        foreach (RoomItem it in riArray)
        {
            it.DestroySelf();
        }

        int count = udList.Count;
        for(int i = 0; i < count; i++)
        {
            GameObject roomItem = Instantiate(roomItemPrefab);
            roomItem.transform.SetParent(roomLayout.transform);
            UserData ud = udList[i];
           // print(ud.Id +","+ ud.userName+ "," + ud.totalCount+ "," + ud.winCount);
            roomItem.GetComponent<RoomItem>().SetRoomInfo(ud.Id,ud.userName, ud.totalCount, ud.winCount,this);
        }
        int roomCount=GetComponentsInChildren<RoomItem>().Length;
       Vector2 size= roomLayout.GetComponent<RectTransform>().sizeDelta;
        roomLayout.GetComponent<RectTransform>().sizeDelta
            =new Vector2(size.x,roomCount* roomItemPrefab.GetComponent<RectTransform>().sizeDelta.y+roomLayout.spacing );
    }

    public void OnJoinClick(int id)
    {
        joinRoomReques.SendRequest(id);
    }

    public void OnJoinResponse(ReturnCode returnCode,UserData ud1,UserData ud2)
    {
        switch (returnCode)
        {
            case ReturnCode.NotFound:
                uiMng.ShowMessageSync("房间被销毁无法加入");
                break;
            case ReturnCode.Fail:
                uiMng.ShowMessageSync("房间已满，无法加入");
                break;
            case ReturnCode.Succese:
                this.ud1 = ud1;this.ud2 = ud2;
                break;
        }
    }

}
