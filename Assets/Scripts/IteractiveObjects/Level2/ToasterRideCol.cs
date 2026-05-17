using UnityEngine;

public class ToasterRideCol : MonoBehaviour
{
    public ToasterLauncher toaster;
    public PlayerMover playerMover;
    public Transform stayOnToasterPos;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (toaster != null)
            toaster.playerSit = true;

        if (playerMover != null && stayOnToasterPos != null)
            playerMover.SitInHighJumper(stayOnToasterPos);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (toaster != null)
            toaster.playerSit = false;
    }
}
