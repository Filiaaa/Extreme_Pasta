using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJumperCol : MonoBehaviour
{
    public HighJumper highJumper;
    public PlayerMover playerMover;
    public Transform stayInJumperPos;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            
            highJumper.playerSit = true;
            playerMover.SitInHighJumper(stayInJumperPos);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        highJumper.playerSit = false;
    }
}
