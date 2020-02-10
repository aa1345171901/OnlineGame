using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour {

    public float speed = 20;
    public RoleType roleType;
    private Rigidbody rigidbody;
    public GameObject explosionEffect;

    public bool isLocal = false;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        rigidbody.MovePosition(transform.position+transform.forward*speed*Time.deltaTime);
	}

    private void OnTriggerEnter(Collider other)
    {
        //print(other.tag);
        if (other.tag == "Player")
        {
            GameFacade.Instance.PlayNormalSound(AudioManager.Sound_ShootPerson);
            if (!isLocal)
            {
                GameFacade.Instance.SendAttack(Random.Range(10, 20));
                Destroy(this.gameObject);
            }
        }
        else
        GameFacade.Instance.PlayNormalSound(AudioManager.Sound_Miss);
        GameObject.Instantiate(explosionEffect, transform.position,transform.rotation);
        Destroy(this.gameObject);
    }
}
