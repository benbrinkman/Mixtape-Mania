using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Studio : MonoBehaviour {


    public AudioSource mainSong;

    public AudioClip[] pop;
	public AudioClip[] rock;
	public AudioSource song1;
	public AudioSource song2;

	private Animator anim;
	public bool isTeam1;
	private float secondCounter;
	private bool recording;
	private bool jamBoost;
	private List <PlayerController> ls;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		secondCounter = Time.time;
		recording = false;
		anim.enabled = false;
		ls = new List<PlayerController> ();

		song2.clip = rock[Random.Range (0, 3)];
		song1.clip = pop[Random.Range (0, 3)];
	}
	
	// Update is called once per frame
	void Update () {

        print(mainSong.isPlaying);

		float incriment = 1.0f;
		foreach ( PlayerController pl in ls){
			if (pl.isJamming() ) {
				incriment += 1.0f;
			}
		}

        float delta = (Time.time - secondCounter);

        if (recording) {


			if (isTeam1) {

				WorldController.pointsTeam1 += incriment * (Time.time - secondCounter);
				
                //print (" player has " + WorldController.pointsTeam1);
			} else {
				WorldController.pointsTeam2 += incriment * (Time.time - secondCounter);
			}
            WorldController.life -= delta;
            secondCounter = Time.time;
		} else {
			song1.Pause ();
			song2.Pause ();
            mainSong.Play();
            //WorldController.mainSong.Play();
		}
	}

	void OnTriggerEnter2D(Collider2D col) {
        if (col.tag == "Song") {
            if (isTeam1)
            {
                song1.Play();
                mainSong.Pause();
                // WorldController.mainSong.Pause();
            }
            else {
                // WorldControl
                mainSong.Pause();// ler.mainSong.Pause();
                song2.Play();
            }
			recording = true;
			anim.enabled = true;

            secondCounter = Time.time;
        }
		if (col.tag == "Player") {
			ls.Add(col.GetComponent<PlayerController>());
		}
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.tag == "Song")
        {
			
            recording = true;
            anim.enabled = true;
        }
    }

	void OnTriggerExit2D(Collider2D col) {
		if (col.tag == "Song"){
			recording = false;
			anim.enabled = false;
		}if (col.tag == "Player") {
			ls.Remove(col.GetComponent<PlayerController>());
		}
	}


}
