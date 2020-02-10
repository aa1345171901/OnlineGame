using Assets.Model;
using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPanel : BasePanel {

    private Text localPlayerUserName;
    private Text localPlayerTotalCount;
    private Text localPlayerWinCount;

    private Text enemyPlayerUserName;
    private Text enemyPlayerTotalCount;
    private Text enemyPlayerWinCount;

    private Transform bluePanel; 
    private Transform redPanel;
    private Transform startButton;
    private Transform quitButton;

    private UserData ud;
    private UserData ud1;
    private UserData ud2;

    private QuitRoomRequest quitRoomRequest;
    private StartGameRequest startGameRequest;

    private bool isPopRoomPanel = false;

    private void Start()
    {
        localPlayerUserName = transform.Find("RedPanel/UserName").GetComponent<Text>();
        localPlayerTotalCount = transform.Find("RedPanel/TotalCount").GetComponent<Text>();
        localPlayerWinCount = transform.Find("RedPanel/WinCount").GetComponent<Text>();
        enemyPlayerUserName = transform.Find("BluePanel/UserName").GetComponent<Text>();
        enemyPlayerTotalCount = transform.Find("BluePanel/TotalCount").GetComponent<Text>();
        enemyPlayerWinCount = transform.Find("BluePanel/WinCount").GetComponent<Text>();

        startButton= transform.Find("StartButton");
        startButton.GetComponent<Button>().onClick.AddListener(OnStartClick);
        quitButton = transform.Find("QuitButton");
        quitButton.GetComponent<Button>().onClick.AddListener(OnQuitClick);
        bluePanel = transform.Find("BluePanel");
        redPanel = transform.Find("RedPanel");

        quitRoomRequest = GetComponent<QuitRoomRequest>();
        startGameRequest = GetComponent<StartGameRequest>();

        EnterAnimi();
    }

    private void Update()
    {
        if (ud != null)
        {
            SetLocalPlayerRes(ud.userName, ud.totalCount.ToString(), ud.winCount.ToString());
            clearEnemyPlayerRes();
            ud = null;
        }
        if (ud1 != null || ud2 != null)
        {
            SetLocalPlayerRes(ud1.userName, ud1.totalCount.ToString(), ud1.winCount.ToString());
            if (ud2 != null)
                SetEnemyPlayerRes(ud2.userName, ud2.totalCount.ToString(), ud2.winCount.ToString());
            else
                clearEnemyPlayerRes();
            ud1 = null;ud2 = null;
        }
        if (isPopRoomPanel)
        {
            uiMng.PopPanel();
            isPopRoomPanel = false;
        }
    }

    public void OnStartClick()
    {
        PlayClickSound();
        startGameRequest.SendRequest();
        startButton.GetComponent<Button>().enabled = false;
    }

    public void OnQuitClick()
    {
        PlayClickSound();
        quitRoomRequest.SendRequest();
    }

    public void OnQuitOnResponse()
    {
        isPopRoomPanel = true;
    }

    public void OnStartResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Fail)
        {
            uiMng.ShowMessageSync("您不是房主，无法开始游戏！");
        }
        else
        {
            uiMng.PushPanelSync(UIPanelType.Game);
            facade.EnterPlayingSync();
        }
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        if (bluePanel!=null)
        {
            EnterAnimi();
            startButton.GetComponent<Button>().enabled = true;
        }
    }

    public override void OnExit()
    {
        QuitAnimi();
    }

    public override void OnPause()
    {
        QuitAnimi();
    }

    public override void OnResume()
    {
        EnterAnimi();
    }

    public void SetLocalPlayerResSync()
    {
        ud = facade.GetUSerData();
    }

    public void SetAllPlayerResSync(UserData ud1,UserData ud2)
    {
        this.ud1 = ud1;
        this.ud2 = ud2;
    }

    /// <summary>
    /// 设置本地面板参数
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="totalCount"></param>
    /// <param name="winCount"></param>
    public  void SetLocalPlayerRes(string userName, string totalCount, string winCount)
    {
        this.localPlayerUserName.text = userName;
        this.localPlayerTotalCount.text = "总场数：" + totalCount.ToString();
        this.localPlayerWinCount.text = "胜利场数：" + winCount.ToString();
    }

    /// <summary>
    /// 设置对手参数
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="totalCount"></param>
    /// <param name="winCount"></param>
    private void SetEnemyPlayerRes(string userName, string totalCount, string winCount)
    {
        this.enemyPlayerUserName.text = userName;
        this.enemyPlayerTotalCount.text = "总场数：" + totalCount.ToString();
        this.enemyPlayerWinCount.text = "胜利场数：" + winCount.ToString();
    }

    /// <summary>
    /// 等待对手
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="totalCount"></param>
    /// <param name="winCount"></param>
    public void clearEnemyPlayerRes()
    {
        this.enemyPlayerUserName.text = "";
        this.enemyPlayerTotalCount.text = "等待玩家加入...";
        this.enemyPlayerWinCount.text =  "";
    }

    private void EnterAnimi()
    {
        gameObject.SetActive(true);
        redPanel.transform.localPosition = new Vector3(-1000, 0, 0);
        redPanel.DOLocalMoveX(-130, 0.5f);
        bluePanel.transform.localPosition = new Vector3(1000, 0, 0);
        bluePanel.DOLocalMoveX(130, 0.5f);
        startButton.localScale = Vector3.zero;
        startButton.DOScale(1,0.5f);
        quitButton.localScale = Vector3.zero;
        quitButton.DOScale(1, 0.5f);
    }

    private void QuitAnimi()
    {
        bluePanel.DOLocalMoveX(1000, 0.5f);
        redPanel.DOLocalMoveX(-1000, 0.5f);
        startButton.DOScale(0, 0.5f);
        quitButton.DOScale(0, 0.5f).OnComplete(()=>gameObject.SetActive(false));
    }
}
