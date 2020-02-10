using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private Animator anim;
    public GameObject arrowPrefab;
    private Transform leftHandTrans;
    private Vector3 dir;
    private PlayerManager playerManager;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        leftHandTrans = transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand/fire");
	}
	
	// Update is called once per frame
	void Update () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            anim.SetBool("Attack", false);
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetBool("Attack", true);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider= Physics.Raycast(ray, out hit);
                if (isCollider)
                {
                    Vector3 targetPoint = hit.point;
                    targetPoint.y = transform.position.y;
                    dir = targetPoint - transform.position;
                    transform.rotation = Quaternion.LookRotation(dir);
                    Invoke("rotate", 0.1f);
                    Invoke("Shoot", 0.3f);
                }
            }
        }
	}

    private void rotate()
    {
        transform.rotation = Quaternion.LookRotation(dir);
    }

    public void SetPlayerManager(PlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
    private void Shoot()
    {
        playerManager.Shoot(arrowPrefab, leftHandTrans.position, Quaternion.LookRotation(dir));
    }
}
