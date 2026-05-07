using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pelmen : MonoBehaviour
{
    public Rigidbody2D playerRb;
    public float jumperSpeed;
    public Vector2 plusForVelocityX;
    public PlayerMover playerMover;
    public Sprite usedPelmen;
    public GameObject particle;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            particle.SetActive(true);
            GetComponent<SpriteRenderer>().sprite = usedPelmen;
            playerMover.SetExtraVelocity(plusForVelocityX);
            playerRb.velocity = new Vector2(1, 5).normalized * jumperSpeed;
        }

    }
}
