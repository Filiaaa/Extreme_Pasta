using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJumper : MonoBehaviour
{
    public AudioSource tomatoSound;
    public PlayerMover playerMover;
    public bool playerSit = false;
    public float jumperSpeed;
    public Animator jumperAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "tomato")
        {
            tomatoSound.Play();
            if (playerSit)
            {
                playerMover.JumpOnHighJumper(jumperSpeed, jumperAnimator);
            }
            else
            {
                jumperAnimator.SetBool("rotate", true);
            }
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
