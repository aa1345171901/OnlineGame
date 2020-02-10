using Common;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePanel : BasePanel {

    private Text timer;
    private int time = -1;

    private Button successButton;
    private Button failButton;
    private Button exitButton;

    private QuitBattleRequest quitBattleRequest;


    private void Start()
    {
        timer = transform.Find("Timer").GetComponent<Text>();
        timer.gameObject.SetActive(false);
        successButton = transform.Find("SuccessButton").GetComponent<Button>();
        successButton.gameObject.SetActive(false);
        failButton = transform.Find("FailButton").GetComponent<Button>();
        failButton.gameObject.SetActive(false);
        exitButton = transform.Find("ExitButton").GetComponent<Button>();
        exitButton.gameObject.SetActive(false);
        successButton.onClick.AddListener(OnResultClick);
        failButton.onClick.AddListener(OnResultClick);
        exitButton.onClick.AddListener(OnExitClick);

        quitBattleRequest = GetComponent<QuitBattleRequest>();
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        failButton.gameObject.SetActive(false);
        successButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (time > -1)
        {
            ShowTime(time);
            time = -1;
        }
    }

    public void OnResultClick()
    {
        uiMng.PopPanel();
        uiMng.PopPanel();
        facade.GameOver();
    }

    public void OnExitClick()
    {
        quitBattleRequest.SendRequest();
    }

    public void OnExitResponse()
    {
        OnResultClick();
    }

    public void ShowTimeSync(int time)
    {
        this.time = time;
    }

    public void ShowTime(int time)
    {
        if (time == 3)
        {
            exitButton.gameObject.SetActive(true);
        }
        timer.gameObject.SetActive(true);
        timer.text = time.ToString();
        timer.transform.localScale = Vector3.one;
        timer.transform.DOScale(2, 0.25f).SetDelay(0.25f);
        timer.transform.DOScale(0, 0.25f).SetDelay(0.25f);
        facade.PlayNormalSound(AudioManager.Sound_Alert);
    }

    public void OnGameOverResponse(ReturnCode returncode)
    {
        Button temp = null;
        switch (returncode)
        {
            case ReturnCode.Succese:
                temp = successButton;
                break;
            case ReturnCode.Fail:
                temp = failButton;
                break;
            default:
                break;
        }
        temp.gameObject.SetActive(true);
        temp.transform.localScale = Vector3.zero;
        temp.transform.DOScale(1, 0.5f);
    }

}
