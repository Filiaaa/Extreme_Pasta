using System.Collections;
using DG.Tweening;
using UnityEngine;

public class IteractiveCola : InventoryObj
{
    public Animator colaAnimator;
    public string fizzStateName = "fizz";
    public float fizzDuration = 1.2f;
    public Transform flyTarget;
    public float flyDuration = 2f;
    public Transform stayOnColaPos;
    public ColaRideCol rideCol;
    public Collider2D interactionTrigger;

    bool _flying;
    bool _used;
    PlayerMover _rider;
    Transform _riderOriginalParent;

    public bool IsFlying => _flying;

    public override void Use(int numberOfItem)
    {
        if (_used)
            return;

        _used = true;

        if (interactionTrigger != null)
            interactionTrigger.enabled = false;
        else
            GetComponent<Collider2D>().enabled = false;

        StartCoroutine(ColaSequence());
    }

    IEnumerator ColaSequence()
    {
        if (colaAnimator != null)
            colaAnimator.Play(fizzStateName, 0, 0f);

        yield return new WaitForSeconds(fizzDuration);

        if (flyTarget == null)
            yield break;

        if (rideCol != null)
            rideCol.enabled = true;

        _flying = true;

        Tween moveTween = transform.DOMove(flyTarget.position, flyDuration).SetEase(Ease.InOutQuad);
        yield return moveTween.WaitForCompletion();

        _flying = false;
        ReleaseRider();
    }

    public void TryAttachRider(PlayerMover player)
    {
        if (!_flying || _rider != null || player == null || stayOnColaPos == null)
            return;

        _rider = player;
        _riderOriginalParent = player.transform.parent;
        player.SitInHighJumper(stayOnColaPos);
        player.transform.SetParent(stayOnColaPos);
        player.transform.localPosition = Vector3.zero;
    }

    void ReleaseRider()
    {
        if (_rider == null)
            return;

        _rider.transform.SetParent(_riderOriginalParent);
        _rider.StayUpAirPlane(flyTarget);
        _rider = null;

        if (rideCol != null)
            rideCol.enabled = false;
    }
}
