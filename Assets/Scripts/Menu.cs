using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

	public SpriteRenderer[] sprt;
	public Sprite[] img;
    private Sprite[] img2;
	private bool[] selected;

	// Use this for initialization
	void Start () {
		selected = new bool[4];
        img2 = new Sprite[4];

		Time.timeScale = 0.0f;

        for (int i = 0; i < 4; i++) {
			selected [i] = false;
            img2[i] = sprt[i].sprite;
        }
	}
	
	// Update is called once per frame
	void Update () {
		bool start = true;
		for (int i = 0; i < 4; i++) {
			if (Input.GetButtonUp ("p" + (i+1) + "_Jump")) {
				selected [i] = !selected[i];

                if (selected[i])
                    sprt[i].sprite = img[i];
                else
                    sprt[i].sprite = img2[i];
			}

            if (!selected[i])
                start = false;
		}
		if (start) {
			for (int i = 0; i < 4; i++) {
				sprt [i].enabled = false;
			}
			GetComponent<SpriteRenderer> ().enabled = false;
			Time.timeScale = 1.0f;
		}
	}
}
