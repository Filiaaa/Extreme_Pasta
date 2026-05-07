using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butter : MonoBehaviour
{
    public PlayerMover Player;
    public float buffingMovingSpeed;
    public float buffingJumpScale;
    public float defaultJumpScale;
    public float defaultMovingSpeed;

    private void Start()
    {
        defaultMovingSpeed = Player.movingSpeed;
        defaultJumpScale = Player.jumpImpulse;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag =="Player")
        {
            Player.movingSpeed = buffingMovingSpeed;
            Player.jumpImpulse = buffingJumpScale;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Player.movingSpeed = defaultMovingSpeed;
            Player.jumpImpulse = defaultJumpScale;

        }
    }
}
