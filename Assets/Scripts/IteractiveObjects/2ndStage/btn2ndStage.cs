using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class btn2ndStage : ItemInScene
{
    public Sprite usedButton;
    public Collider2D oldCol;
    public Collider2D newCol;
    public AudioSource buttonSound;
    public Animator water;
    public GameObject[] ice;
    public GameObject steam;
    public float spawnTime;
    float time;
    public bool firstTime = false;

    [SerializeField] private StartLevelScript startLevelScript;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            oldCol.enabled = false;
            newCol.enabled = true;
            GetComponent<SpriteRenderer>().sprite = usedButton;
            caseNumb = startLevelScript.settedObjescts[0].Key;
            switch (caseNumb)
            {
                case 0:
                    if (firstTime)
                    {
                        firstTime = false;
                        buttonSound.Play();
                    }
       
                    water.SetBool("hot", false);
                    water.SetBool("changed", true);
                    foreach (var obj in ice)
                    {

                        obj.SetActive(true);
                        StartCoroutine(Spawn(obj.GetComponent<SpriteRenderer>()));
                    }

                    break;
                case 1:
                    buttonSound.Play();
                    water.SetBool("changed", true);
                    water.SetBool("hot", true);
                    steam.SetActive(true);
                    StartCoroutine(Spawn(steam.GetComponent<SpriteRenderer>()));
                    break;
                default:
                    break;
            }

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
