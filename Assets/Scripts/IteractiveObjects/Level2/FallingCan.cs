using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FallingCan : MonoBehaviour
{
    public ToasterLauncher toaster;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (toaster == null)
            return;

        Collider2D other = collision.collider;
        if (other == null)
            return;

        if (other.CompareTag("toasterHandle") || other.GetComponent<ToasterLauncher>() != null)
            toaster.TryLaunchFromCan(collision.collider);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (toaster == null)
            return;

        if (collision.CompareTag("toasterHandle"))
            toaster.TryLaunchFromCan(collision);
    }
}
