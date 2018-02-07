using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JamDelete : MonoBehaviour {
		
	private PlayerController cont;

	// Use this for initialization
	void Start () {
		cont = GetComponentInParent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!cont.jamming) {
			Destroy (gameObject);
		}
	}
}
