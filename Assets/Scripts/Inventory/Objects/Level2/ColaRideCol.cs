using UnityEngine;

public class ColaRideCol : MonoBehaviour
{
    public IteractiveCola cola;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        PlayerMover player = collision.GetComponent<PlayerMover>();
        if (player != null)
            cola.TryAttachRider(player);
    }
}
