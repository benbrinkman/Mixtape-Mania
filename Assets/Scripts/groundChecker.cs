using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundChecker : MonoBehaviour {

	private PlayerController cont;
	private Collider2D col;
    public Collider2D platformCollider;

    // Use this for initialization
    void Start () {
		cont = GetComponentInParent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            GroundPlayer(true);
        }
        if (col.tag == "Platform" && col.isTrigger == false)
        {
            GroundPlayer(true);
            platformCollider = col;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Ground")
        {
            GroundPlayer(false);
        }
        if (col.tag == "Platform" && col.isTrigger == false)
        {
            GroundPlayer(false);
            platformCollider = null;
        }
    }

    void GroundPlayer(bool isGrounded)
    {
        cont.grounded = isGrounded;
        cont.anim.SetBool("isJumping", !isGrounded);
    }
}
