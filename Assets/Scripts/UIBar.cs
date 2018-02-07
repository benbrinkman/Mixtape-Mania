using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBar : MonoBehaviour {

	public bool isTeam1;
	private float noFilled;
	private float incriment;

	// Use this for initialization
	void Start () {
		float add;
		if (isTeam1) {
			add = -29;
		} else {
			add = 29;
		}
		noFilled = add + -10.81f;
		incriment = (10.81f /WorldController.pointsToWin);
	}
	
	// Update is called once per frame
	void Update () {
        if (isTeam1) {
			this.transform.position = new Vector3 (noFilled + (incriment * WorldController.pointsTeam1), transform.position.y, transform.position.z);
		} else {
			this.transform.position = new Vector3 (noFilled + (incriment * WorldController.pointsTeam2), transform.position.y, transform.position.z);
		}
	}
}
