using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : BaseManager {

    private GameObject cameraGo;
    private FollowTarget followTarget;
    private Animator anim;
    private Vector3 cameraPosition;
    private Vector3 cameraRotation;

    public CameraManager(GameFacade facade) : base(facade) { }

    public override void OnInit()
    {
        cameraGo = Camera.main.gameObject;
        followTarget = cameraGo.GetComponent<FollowTarget>();
        anim = cameraGo.GetComponent<Animator>();
        anim.enabled = true;
        followTarget.enabled = false;
    }

    public void FollowRole(Transform target)
    {
        anim.enabled = false;
        followTarget.target = target.transform;
        followTarget.IsGame = true;
        cameraPosition = cameraGo.transform.position;
        cameraRotation = cameraGo.transform.eulerAngles;
        followTarget.enabled = true;
    }

    public void WalkThrouthScene()
    {
        followTarget.enabled = false;
        followTarget.IsGame = false;
        cameraGo.transform.DOMove(cameraPosition, 1f);
        cameraGo.transform.DORotate(cameraRotation, 1f).OnComplete(delegate()
        {
            anim.enabled = true;
        });
    }

}
