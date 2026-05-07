using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheesegrater : InventoryObj
{

    public GameObject cheese;
    public GameObject mountainOfCheese;
    float time = 0;
    public float spawnTime = 1;

    public override void Use(int numberOfItem)
    {
        mountainOfCheese.SetActive(true);
        StartCoroutine(Spawn(mountainOfCheese.GetComponent<SpriteRenderer>()));
    }

    Color newColor;
    IEnumerator Spawn(SpriteRenderer sr)
    {
        while (sr.color.a < 1)
        {
            time += Time.deltaTime;
            newColor = sr.color;
            newColor.a = (time / spawnTime) * 1;
            sr.color = newColor;
            yield return new WaitForEndOfFrame();
        }

    }
}
