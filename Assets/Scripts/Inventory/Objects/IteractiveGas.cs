using UnityEngine;

public class IteractiveGas : InventoryObj
{
    public AudioSource waterOnFireSound;
    public GameObject waterAfterSpil;
    public int newNumberOfNeedItem;
    public Animator cupAnimator;
    public Transform newMovingToPoint;
    public Animator[] gases;
    public GameObject gasDeathCol;
    public GameObject[] lightForGases;

    public override void Use(int numberOfItem)
    {
        if (numberOfItem == newNumberOfNeedItem)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            waterAfterSpil.SetActive(false);
        }
        else
        {
            waterOnFireSound?.Play();

            foreach (var gas in gases)
                gas.SetBool("burn", false);
            foreach (var light in lightForGases)
                light.SetActive(false);

            gasDeathCol?.SetActive(false);
            movingToPoint = newMovingToPoint;
            cupAnimator.gameObject.SetActive(true);
            cupAnimator?.SetBool("spil", true);
            numberOfNeedItem = newNumberOfNeedItem;
            waterAfterSpil?.SetActive(true);

            inventorySys.requiredItemID = newNumberOfNeedItem;
        }
    }
}