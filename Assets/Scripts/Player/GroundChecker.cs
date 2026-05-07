using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    public Transform groundCheckPoint;
    public float checkRadius = 0.15f;
    public LayerMask groundLayer;
    public PlayerMover player;

    private void FixedUpdate()
    {
        if (groundCheckPoint == null || player == null) return;
        player.onGround = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
        }
    }
}