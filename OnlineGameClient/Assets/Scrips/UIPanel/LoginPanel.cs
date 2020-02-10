using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Common;

public class LoginPanel : BasePanel {

    private Button closeButton;
    private InputField usernameIF;
    private InputField passwordIF;

    private LoginRequest loginRequest;


    private void Start()
    {
        usernameIF = transform.Find("UseLabel/UsernameInputField").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/PasswoedInputField").GetComponent<InputField>();

        closeButton = GameObject.Find("CloseButton").GetComponent<Button>();
        closeButton.onClick.AddListener(OnCloseClick);

        transform.Find("loginButton").GetComponent<Button>().onClick.AddListener(OnLoginClick);
        transform.Find("RegistButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);

        loginRequest = GetComponent<LoginRequest>();
    }

    /// <summary>
    /// 播放进场动画，以及获得组件
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        EnterAnimation();

    }

    public void OnCloseClick()
    {
        PlayClickSound();
        uiMng.PopPanel();
    }

    public override void OnPause()
    {
        HideAnimation();
    }

    public override void OnResume()
    {
        EnterAnimation();
    }

    public override void OnExit()
    {
        base.OnExit();
        HideAnimation();
    }

    public void OnLoginClick()
    {
        PlayClickSound();
        string msg = "";
        if(string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空！";
        }
        if(string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "密码不能为空！";
        }
        if (msg != "")
        {
            uiMng.ShowMessage(msg);
        }
        //发送用户名密码至服务器验证
        if(!string.IsNullOrEmpty(usernameIF.text))
        loginRequest.SendRequest(usernameIF.text, passwordIF.text);
    }

    public void OnRegisterClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Register);
    }


    public void OnLoginResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Succese)
        {
            uiMng.PushPanelSync(UIPanelType.RoomList);
            //登陆成功
        }
        else
        {
            uiMng.ShowMessageSync("用户名或密码错误，无法登陆！");
        }
    }

    public void EnterAnimation()
    {
        gameObject.SetActive(true);
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(1, 1);
    }

    public void HideAnimation()
    {
        Tween tween = transform.DOScale(0, 0.5f);
        tween.OnComplete(() => gameObject.SetActive(false));
    }
}
