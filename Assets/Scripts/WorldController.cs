using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour {

	

	public Transform[] spawnPoints;
	public int numSpawns;
	public static float pointsTeam1;
	public static float pointsTeam2;
	public static float pointsToWin = 120;
	public GameObject song;


	public SpriteRenderer team1Win;
	public SpriteRenderer team2Win;
	public SpriteRenderer showWin;
	public float end;

	public static float life;
    public static float lifeMax;
	public static float secondCounter;

	// Use this for initialization
	void Start () {
        
		showWin.enabled = false;
		team1Win.enabled = false;
		team2Win.enabled = false;
		pointsToWin = 120;
		secondCounter = Time.time;
		lifeMax = 30;
        life = lifeMax;
		pointsTeam1 = 0;
		pointsTeam2 = 0;
		Instantiate(song, spawnPoints [Random.Range(0, numSpawns-1)].transform.position, Quaternion.Euler(new Vector3(0, 0, 45)));

	}

	// Update is called once per frame
	void Update () {

		if (pointsTeam1 >= pointsToWin) {
			Time.timeScale = 0;
			team1Win.enabled = true;
			showWin.enabled = true;
			end++;
		}
		if (pointsTeam2 >= pointsToWin) {
			team2Win.enabled = true;
			showWin.enabled = true;
			Time.timeScale = 0;
			end++;
		}


		if (end > 500) {
			print ("quit");
			Application.Quit ();
		}
        
		if (life < 0) {
			Instantiate(song, spawnPoints [Random.Range(0, numSpawns-1)].transform.position, Quaternion.Euler(new Vector3(0, 0, 45)));
            WorldController.life = WorldController.lifeMax;
			secondCounter = Time.time;
		}
		//print (”" + pointsTeam1);
	}
}
