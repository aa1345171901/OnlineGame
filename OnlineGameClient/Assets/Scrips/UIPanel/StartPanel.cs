using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StartPanel : BasePanel  {

    private Button loginButton;
    private Animator anim;

    public override void OnEnter()
    {
        base.OnEnter();
        loginButton = transform.Find("LoginButton").GetComponent<Button>();
        anim = loginButton.GetComponent<Animator>();
        loginButton.onClick.AddListener(OnLoginClick);
    }

    private void OnLoginClick()
    {
        PlayClickSound();
        uiMng.PushPanel(UIPanelType.Login);
    }

    public override void OnPause()
    {
        base.OnPause();
        anim.enabled = false;
        loginButton.transform.DOScale(0, 0.5f).OnComplete(() => loginButton.gameObject.SetActive(false));
    }

    public override void OnResume()
    {
        base.OnResume();
        loginButton.gameObject.SetActive(true);
        loginButton.transform.DOScale(1, 0.5f).OnComplete(() => anim.enabled = true);
    }
}
