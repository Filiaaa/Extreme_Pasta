using UnityEngine;

public class InventoryObj : MonoBehaviour
{
    public int numberOfNeedItem;
    public InventorySys inventorySys;
    public Transform movingToPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartInteraction();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EndInteraction();
        }
    }

    void StartInteraction()
    {
        inventorySys.StartInteraction(this, numberOfNeedItem, movingToPoint);
    }

    void EndInteraction()
    {
        inventorySys.EndInteraction();
    }

    public virtual void Use(int numberOfItem)
    {
        Debug.Log($"Base Use called with item {numberOfItem}");
    }
}