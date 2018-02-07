using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chargeAttack : MonoBehaviour {

	public GameObject blood;
	public PlayerController thisPlayer;
	public PlayerController otherPlayer;
	private float hitForce;
    public bool attacking;

	// Use this for initialization
	void Start () {
		hitForce = 10000;
		thisPlayer = GetComponentInParent<PlayerController> ();
        attacking = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerStay2D(Collider2D col) {
        
		if (attacking && col.tag == "Player"){
			otherPlayer = col.GetComponent<PlayerController> ();
			if (otherPlayer.team != thisPlayer.team && !otherPlayer.isBlocking()) {
				//print ("I hit player: " + otherPlayer.getPlayerID ());
				Instantiate(blood, this.transform.position, Quaternion.identity);

                /*
                if (thisPlayer.facingLeft) {
					otherPlayer.rb2d.velocity = new Vector2(-hitForce, 10f);
				} else {
					otherPlayer.rb2d.velocity = new Vector2(hitForce, 10f);
				}
                */

                Vector2 dif = otherPlayer.transform.position - thisPlayer.transform.position;
                dif.Normalize();
                dif *= hitForce;

                otherPlayer.rb2d.velocity = dif;

                otherPlayer.stun(0.5f);
                otherPlayer.dropSong = true;
            }
		}
	}

    public void SetHitForce(float newHitForce)
    {
        hitForce = newHitForce;
    }
}
