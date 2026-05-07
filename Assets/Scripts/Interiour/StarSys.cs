using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StarSys : MonoBehaviour
{
    [SerializeField] private Image[] images;
    [SerializeField] private Sprite filledStar;
    [SerializeField] private Sprite defaultStar;
    [SerializeField] private float moveDuration = 0.5f;

    [Header("Timing")]
    [SerializeField] private float spawnTime = 0.5f;
    [SerializeField] private float waitBeforeDespawn = 0.5f;

    [SerializeField] private Text[] texts;

    private int starNumb = 0;
    private Sequence uiFadeSequence;

    private void Awake()
    {
        DOTween.Init();
    }

    public void MoveToStarPoint(GameObject star)
    {
        if (starNumb >= images.Length) return;

        int currentStarIndex = starNumb;
        Transform targetPoint = images[currentStarIndex].transform;
        EnableStarText();


        star.transform.DOMove(targetPoint.position, moveDuration).OnComplete(() =>
        {
            star.SetActive(false);

            images[currentStarIndex].sprite = filledStar;
            Animator anim = images[currentStarIndex].GetComponent<Animator>();
            if (anim != null)
                anim.SetBool("fiilling", true);


            starNumb++;

            if (images.Length > 0 && images[0].color.a < 1f)
            {
                PlayUIFadeSequence();
            }
        });
    }

    private void EnableStarText()
    {
        for (int i = 0; i < texts.Length; i++)
        {
            if (i != starNumb)
            {
                texts[i].enabled = false;
            }
            else
            {
                texts[i].enabled = true;
            }
        }
    }

    private void PlayUIFadeSequence()
    {
        if (uiFadeSequence != null && uiFadeSequence.IsActive())
        {
            uiFadeSequence.Kill();
        }

        uiFadeSequence = DOTween.Sequence();

        uiFadeSequence.AppendCallback(() => FadeUI(1f, spawnTime));

        uiFadeSequence.AppendInterval(waitBeforeDespawn);

        uiFadeSequence.AppendCallback(() => FadeUI(0f, spawnTime));
    }

    private void FadeUI(float targetAlpha, float duration)
    {
        foreach (var im in images)
        {
            im.DOFade(targetAlpha, duration);
        }

        foreach (var t in texts)
        {
            t.DOFade(targetAlpha, duration);
        }
    }

    public void SetToDefault()
    {
        starNumb = 0;

        foreach (var image in images)
        {
            image.sprite = defaultStar;
            Animator anim = image.GetComponent<Animator>();
            if (anim != null)
                anim.SetBool("fiilling", false);

            Color c = image.color;
            c.a = 1f;
            image.color = c;
        }

        foreach (var t in texts)
        {
            Color c = t.color;
            c.a = 1f;
            t.color = c;
        }

        if (uiFadeSequence != null && uiFadeSequence.IsActive())
        {
            uiFadeSequence.Kill();
        }

        uiFadeSequence = DOTween.Sequence();
        uiFadeSequence.AppendInterval(waitBeforeDespawn);
        uiFadeSequence.AppendCallback(() => FadeUI(0f, spawnTime));
    }
}