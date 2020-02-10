using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Common;

public class RegisterPanel : BasePanel
{

    private InputField usernameIF;
    private InputField passwordIF;
    private InputField repasswordIF;

    private RegisterRequest registerRequest;

    private void Start()
    {
        usernameIF = transform.Find("UseLabel/UsernameInputField").GetComponent<InputField>();
        passwordIF = transform.Find("PasswordLabel/UsernameInputField").GetComponent<InputField>();
        repasswordIF = transform.Find("RePasswordLabel/UsernameInputField").GetComponent<InputField>();

         transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(OnCloseClick);
         transform.Find("RegistButton").GetComponent<Button>().onClick.AddListener(OnRegisterClick);

        registerRequest = GetComponent<RegisterRequest>();
    }

    public override void OnEnter()
    {
        gameObject.SetActive(true);
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(1, 1);
    }

    public void OnCloseClick()
    {
        PlayClickSound();
        Tween tween = transform.DOScale(0, 0.5f);
        tween.OnComplete(() => uiMng.PopPanel());
    }

    public void OnRegisterClick()
    {
        PlayClickSound();
        string msg = "";
        if (string.IsNullOrEmpty(usernameIF.text))
        {
            msg += "用户名不能为空！";
        }
        if (string.IsNullOrEmpty(passwordIF.text))
        {
            msg += "\n密码不能为空！";
        }
       else  if (passwordIF.text != repasswordIF.text)
        {
            msg += "\n密码不一致";
        }    
        if(msg!="")
        {
            uiMng.ShowMessage(msg);
        }
        //向服务器发送注册请求
        registerRequest.SendRequest(usernameIF.text, passwordIF.text);
    }
    public override void OnExit()
    {
        base.OnExit();
        gameObject.SetActive(false);
    }

    public void OnRegisterResponse(ReturnCode returnCode)
    {
        if (returnCode == ReturnCode.Succese)
        {
            uiMng.ShowMessageSync("注册成功!");
        }
        else
        {
            uiMng.ShowMessageSync("注册失败，用户名重复!");
        }
    }

}
