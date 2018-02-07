using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongScript : MonoBehaviour {


    public AudioSource soundBit;

	private float secondCounter;
	private float life;
	public bool pickedUp;
	private PlayerController cont;
	private float coolDownTimer;
	private float coolDownLength;



	// Use this for initialization
	void Start () {
		

		secondCounter = Time.time;
		coolDownLength = 1;
		coolDownTimer = Time.time -coolDownLength;
		pickedUp = false;
	}
	
	// Update is called once per frame
	void Update () { 
        if (WorldController.life < 0) {
            if (pickedUp) {
				cont = GetComponentInParent<PlayerController> ();
				cont.hasSong = false;
			}
			Destroy (this.gameObject);
		}

		if (!pickedUp) {
			this.gameObject.transform.parent = null;
		} else {
			
			if (cont.dropSong == true) {
				GetComponentInChildren<PassiveMove> ().enable = true;
				coolDownTimer = Time.time;
				pickedUp = false;
				cont.dropSong = false;
				cont.hasSong = false;
                GetComponentInChildren<SpriteRenderer>().enabled = true;
            }
		}
	}


	void OnTriggerEnter2D(Collider2D col) {
		if (col.tag == "Player" && ((Time.time - coolDownTimer) > coolDownLength) && !pickedUp) {
            transform.parent = col.transform;
			transform.position = new Vector3(col.transform.position.x, col.transform.position.y, 3);
			GetComponent<AudioSource> ().Play ();
            GetComponentInChildren<SpriteRenderer>().enabled = false;

            cont = GetComponentInParent<PlayerController> ();
			cont.dropSong = false;
			cont.hasSong = true;
			pickedUp = true;
			GetComponentInChildren<PassiveMove> ().enable = false;
		} 
	}
}
