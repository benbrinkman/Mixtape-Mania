using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    public BoxCollider2D upperPlatform;
    public BoxCollider2D lowerPlatform;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Physics2D.IgnoreCollision(upperPlatform, col);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            Physics2D.IgnoreCollision(upperPlatform, col, false);
        }
    }
}
