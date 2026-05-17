using UnityEngine;

public class FridgeIcePickup : MonoBehaviour
{
    public int iceItemId = 6;
    public Sprite iceIcon;
    public string iceItemName = "ice";
    public InventorySys inventory;
    public AudioSource pickupSound;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (inventory == null || inventory.HasItem(iceItemId))
            return;

        ItemData itemData = new ItemData
        {
            id = iceItemId,
            itemName = iceItemName,
            icon = iceIcon,
            maxStack = 1
        };

        if (inventory.PickupWorldItem(itemData, transform.position))
         {
            if (pickupSound != null)
                pickupSound.Play();
         }
    }
}
