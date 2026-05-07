using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Btn1StStage : ItemInScene
{
    public Sprite usedButton;
    public Collider2D oldCol;
    public Collider2D newCol;
    public AudioSource buttonSound;
    public GameObject gasFireCol;
    public Animator[] gas;
    public GameObject[] lightForGas;
    public bool firstTime = false;

    

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            switch (caseNumb)
            {
                case 0:
                    oldCol.enabled = false;
                    newCol.enabled = true;
                    GetComponent<SpriteRenderer>().sprite = usedButton;
                    if (firstTime)
                    {
                        firstTime = false;
                        buttonSound.Play();
                    }

                    foreach (var item in gas)
                    {
                        item.SetBool("burn", false);
                    }
                    gasFireCol.GetComponent<BoxCollider2D>().enabled = false;
                    gameObject.GetComponent<Btn1StStage>().enabled = false;
                    foreach (var light in lightForGas)
                    {
                        light.SetActive(false);
                    }
                    break;
                default:
                    break;
            }
        }


    }
}
