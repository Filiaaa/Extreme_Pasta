using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DoughRise1stStage : ItemInScene
{
    static DoughRise1stStage _active;

    public SpriteRenderer packetRenderer;
    public Sprite sealedSprite;
    public Sprite tornSprite;
    public Transform dough;
    public Animator doughAnimator;
    public string riseStateName = "rise";
    public float riseHeight = 2f;
    public float riseDuration = 2f;
    public bool useAnimator = true;

    bool _done;

    void OnEnable()
    {
        _active = this;
        if (dough != null)
            dough.gameObject.SetActive(false);
    }

    void OnDisable()
    {
        if (_active == this)
            _active = null;
    }

    public static void OnKnifeCutPacket()
    {
        if (_active != null)
            _active.CutAndRise();
    }

    void CutAndRise()
    {
        if (_done)
            return;
        _done = true;

        if (packetRenderer != null && tornSprite != null)
            packetRenderer.sprite = tornSprite;

        if (dough == null)
            return;

        dough.gameObject.SetActive(true);

        if (useAnimator && doughAnimator != null)
        {
            doughAnimator.Play(riseStateName, 0, 0f);
            return;
        }

        float targetY = dough.position.y + riseHeight;
        dough.DOMoveY(targetY, riseDuration).SetEase(Ease.OutQuad);
    }
}
