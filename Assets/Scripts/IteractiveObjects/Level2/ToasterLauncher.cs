using UnityEngine;

public class ToasterLauncher : MonoBehaviour
{
    public Animator toasterAnimator;
    public string popStateName = "pop";
    public PlayerMover player;
    public Vector2 playerImpulse = new Vector2(8f, 6f);
    public AudioSource popSound;

    [HideInInspector]
    public bool playerSit;

    bool _fired;

    public void TryLaunchFromCan(Collider2D canCollider)
    {
        if (_fired)
            return;

        _fired = true;
        if (popSound != null)
            popSound.Play();
        if (toasterAnimator != null)
            toasterAnimator.Play(popStateName, 0, 0f);

        if (playerSit)
            LaunchPlayer();
    }

    void LaunchPlayer()
    {
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.GetComponent<PlayerMover>();
        }

        if (player == null)
            return;

        playerSit = false;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb == null)
            return;

        player.sittingInJumper = false;
        rb.isKinematic = false;
        player.SetExtraVelocity(playerImpulse);
        rb.velocity = playerImpulse;
    }
}
