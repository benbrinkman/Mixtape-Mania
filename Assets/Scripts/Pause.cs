using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

	public SpriteRenderer show;
	public SpriteRenderer child;
	public Transform childMove;
	private bool topSelected;
	private float cooldown;


	// Use this for initialization
	void Start () {
		cooldown = Time.time;
		show = GetComponent<SpriteRenderer> ();
		show.enabled = false;
		child.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Pause")) {
			Time.timeScale = 0.0f;
			show.enabled = true;
			child.enabled = true;
		}
        if (show.enabled)
        {
            if (topSelected && Input.GetButtonDown("Submit"))
            {
                Application.Quit();
            }
            else if (!topSelected && Input.GetButtonDown("Submit"))
            {
                Time.timeScale = 1.0f;
                show.enabled = false;
                child.enabled = false;
            }
            else if (Input.GetAxis("Input_Vertical") != 0 && (Time.time - cooldown > 0.2))
            {
                cooldown = Time.time;
                if (topSelected)
                {
                    topSelected = false;
                    childMove.localPosition = new Vector3(0.0f, 0.2f, 0.0f);
                }
                else
                {
                    topSelected = true;
                    childMove.localPosition = new Vector3(0.0f, 0.97f, 0.0f);
                }
            }
        }

	}
}
