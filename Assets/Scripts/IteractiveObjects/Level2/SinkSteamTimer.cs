using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SinkSteamTimer : ItemInScene
{
    public GameObject steam;
    public GameObject[] iceObjects;
    public float steamFadeInDuration = 1f;

    void Start()
    {
        StartCoroutine(WaitThenSteam());
    }

    IEnumerator WaitThenSteam()
    {
        yield return new WaitForSeconds(waitTime);

        if (iceObjects != null)
        {
            foreach (GameObject ice in iceObjects)
            {
                if (ice != null)
                    ice.SetActive(false);
            }
        }

        if (steam == null)
        {
            enabled = false;
            yield break;
        }

        steam.SetActive(true);

        SpriteRenderer sr = steam.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color target = sr.color;
            target.a = 1f;
            Color transparent = target;
            transparent.a = 0f;
            sr.color = transparent;
            sr.DOColor(target, steamFadeInDuration);
        }

        enabled = false;
    }
}
