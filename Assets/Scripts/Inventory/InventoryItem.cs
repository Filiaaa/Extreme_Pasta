using UnityEngine;
using DG.Tweening;

public class InventoryItem : MonoBehaviour
{
    [Header("Item Data")]
    public int numberOfItem;
    public Sprite slotSprite;
    public int maxStack = 1;

    [Header("References")]
    public InventorySys inventory;
    public Transform playerTransform;

    [Header("Animation")]
    public float flyDuration = 0.5f;

    private bool alreadyTaken = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !alreadyTaken)
        {
            alreadyTaken = true;
            PickupItem();
        }
    }

    void PickupItem()
    {
        GetComponent<Collider2D>().enabled = false;

        ItemData itemData = new ItemData
        {
            id = numberOfItem,
            itemName = gameObject.name,
            icon = slotSprite,
            maxStack = maxStack
        };

        bool added = inventory.AddItem(itemData, 1);
        GetComponent<SpriteRenderer>().enabled = !added;

        if (added)
        {
            int slotIndex = -1;
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if (!inventory.slots[i].IsEmpty &&
                    inventory.slots[i].item.id == itemData.id)
                {
                    slotIndex = i;
                    break;
                }
            }

            if (slotIndex != -1)
            {
                inventory.AnimatePickupToWorldSlot(
                    itemData,
                    transform.position,
                    slotIndex,
                    () => Destroy(gameObject)
                );
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("Инвентарь полон!");
            Destroy(gameObject);
        }
    }
}