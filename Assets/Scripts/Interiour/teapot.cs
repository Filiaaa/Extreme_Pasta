using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teapot : MonoBehaviour
{
    public AudioSource sound;
    public float velocityNeedToBalance;
    public float maxVelocityScale;
    public float minVelocity;
    public Rigidbody2D playerRb;
    public Transform endPoint;
    float velocityScale = 0;
    float maxDist;
    float defaultPlayerGravity;
    private void Start()
    {
        maxDist = Vector2.Distance(transform.position, endPoint.position);
        defaultPlayerGravity = playerRb.gravityScale;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            sound.Play();
            playerRb.gravityScale = 0;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            playerRb.gravityScale = defaultPlayerGravity;
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        
        if (collision.tag == "Player")
        {
            if (Vector2.Distance(transform.position, playerRb.transform.position) >= maxDist)
            {
                velocityScale = velocityNeedToBalance;
            }
            else
            {
                velocityScale = (maxDist - Vector2.Distance(transform.position, playerRb.transform.position)) / maxDist * maxVelocityScale;
                if (velocityScale <= minVelocity)
                {
                    velocityScale = minVelocity;
                }
            }

            playerRb.velocity = new Vector2(playerRb.velocity.x, velocityScale);
        }
    }
}
