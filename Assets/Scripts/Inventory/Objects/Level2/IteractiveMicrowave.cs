using System.Collections;
using DG.Tweening;
using UnityEngine;

public class IteractiveMicrowave : InventoryObj
{
    public Animator microwaveAnimator;
    public string animationStateName = "timer";
    public GameObject popcornMountain;
    public float mountainFadeInDuration = 1f;

    bool _used;

    public override void Use(int numberOfItem)
    {
        if (_used)
            return;

        _used = true;
        GetComponent<Collider2D>().enabled = false;

        if (microwaveAnimator != null)
            microwaveAnimator.Play(animationStateName, 0, 0f);

        StartCoroutine(PlayMicrowaveThenShowMountain());
    }

    IEnumerator PlayMicrowaveThenShowMountain()
    {
        if (microwaveAnimator != null)
        {
            yield return null;
            while (true)
            {
                AnimatorStateInfo state = microwaveAnimator.GetCurrentAnimatorStateInfo(0);
                if (state.IsName(animationStateName) && state.normalizedTime >= 1f && !microwaveAnimator.IsInTransition(0))
                    break;
                yield return null;
            }
        }

        if (popcornMountain == null)
            yield break;

        popcornMountain.SetActive(true);

        SpriteRenderer sr = popcornMountain.GetComponent<SpriteRenderer>();
        if (sr == null)
            yield break;

        Color targetColor = sr.color;
        targetColor.a = 1f;
        Color transparent = targetColor;
        transparent.a = 0f;
        sr.color = transparent;

        sr.DOColor(targetColor, mountainFadeInDuration);
    }
}
