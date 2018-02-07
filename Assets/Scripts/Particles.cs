using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {

	private GameObject rs;
	private float endTime;

	// Use this for initialization
	void Start () {
		endTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time - endTime > 3) {
			Destroy (this.gameObject);
		}
	}
}
