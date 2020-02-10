using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    public float speed = 3;
    private Animator anim;

    private KeyCode jump = KeyCode.Space;

    private Rigidbody rigidbody;

    public  bool OnGround = true;

    public float forward;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(h, 0, v) * speed * Time.deltaTime, Space.World);
        
        if(v!=0||h!=0)
        transform.rotation = Quaternion.LookRotation(new Vector3(h, 0, v));
        forward = Mathf.Max(Mathf.Abs(h), Mathf.Abs(v));
        anim.SetFloat("Forward", forward);

        if (Input.GetKeyDown(jump)&& OnGround)
        {
            rigidbody.velocity = Vector3.up*speed*1.5f;
            OnGround=false;
        }
        if (!OnGround)
        {
            anim.SetBool("OnGround", false);
        }
        else
        {
            anim.SetBool("OnGround", true);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            OnGround = true;
        }
    }
}
