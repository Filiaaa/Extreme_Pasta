using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirplaneCol : MonoBehaviour
{
    public AudioSource tomatoSound;
    public Airplane airplane;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tomato")
        {
            tomatoSound.Play();
            airplane.Fly();
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
