using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseStayer : MonoBehaviour
{
    public Cheesegrater cheesegrater;
    public PlayerMover playerMover;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
/*            cheesegrater.StayOnCheese();*/
        }
    }
}
