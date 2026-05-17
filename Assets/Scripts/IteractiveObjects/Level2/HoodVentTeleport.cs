using UnityEngine;

public class HoodVentTeleport : MonoBehaviour
{
    public Transform destination;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || destination == null)
            return;

        collision.transform.position = destination.position;
    }
}
