using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class String : MonoBehaviour
{

    public Transform upPoint;
    public Transform downPoint;
    public PlayerMover player;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (player.transform.position.x >= transform.position.x)
            {
                player.distBetwPlayerAndString = Mathf.Abs(player.distBetwPlayerAndString);
            }
            else
            {
                player.distBetwPlayerAndString = -Mathf.Abs(player.distBetwPlayerAndString);
            }
            
            player.upStringPoint = upPoint;
            player.downStringPoint = downPoint;
            player.SetClimbingMode(gameObject);
        }
    }

}
