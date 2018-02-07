using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveMove : MonoBehaviour {

	private float height;
	private Vector3 startPos;
	public bool enable;
	// Use this for initialization
	void Start () {
		enable = true;
		height = 0.3f;
		startPos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (enable) {
			this.transform.position = startPos + new Vector3 (0, Mathf.Sin (Time.time) * height, 0);
		}
        if (!enable) {
            startPos = this.transform.position;
		}
	}
}
