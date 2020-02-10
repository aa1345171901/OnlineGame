using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    public Transform target;
    private Vector3 offset=new Vector3(1,9.7f,-9.5f) ;
    //private Vector3 offset=new Vector3(1,1.35f,-1.66f) ;
    public float smooth = 2;

    public bool IsGame=false;


    private void Update()
    {
        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPosition, smooth * Time.deltaTime);
        if (IsGame)
        {
            if (Input.GetMouseButton(1))
            {
                float _mouseX = Input.GetAxis("Mouse X");
                float _mouseY = Input.GetAxis("Mouse Y");
                Camera.main.transform.RotateAround(targetPosition, Vector3.up, _mouseX*10);
                Camera.main.transform.RotateAround(targetPosition, Camera.main.transform.right, -_mouseY * 10);
            }
            else
            {
                transform.DOLookAt(target.position, 1f);
            }
        }
    }
}
