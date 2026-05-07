using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public StarSys starSys;
    public AudioSource coinSound;
    public bool firstTime = true;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && firstTime)
        {
            firstTime = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            coinSound.Play();
            starSys.MoveToStarPoint(gameObject);
        }
    }
}
