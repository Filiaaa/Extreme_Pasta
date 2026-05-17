using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBtn : ItemInScene
{
    public Sprite usedButton;
    public Collider2D oldCol;
    public Collider2D newCol;
    public AudioSource buttonSound;
    public Animator water;
    public GameObject steam;
    public float spawnTime;
    float time;
    public bool firstTime = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            oldCol.enabled = false;
            newCol.enabled = true;
            GetComponent<SpriteRenderer>().sprite = usedButton;
           
           buttonSound.Play();
                    water.SetBool("changed", true);
                    water.SetBool("hot", true);
                    steam.SetActive(true);
                    StartCoroutine(Spawn(steam.GetComponent<SpriteRenderer>()));

        }
    }
    Color newColor;
    IEnumerator Spawn(SpriteRenderer sr)
    {
        while (sr.color.a  < 1)
        {
            time += Time.deltaTime;
            newColor = sr.color;
            newColor.a = (time / spawnTime) * 1;
            sr.color = newColor;
            yield return new WaitForEndOfFrame();
        }
        gameObject.GetComponent<btn2ndStage>().enabled = false;
    }
}
