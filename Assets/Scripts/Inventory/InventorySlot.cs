[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int quantity = 0;

    public bool IsEmpty => item == null || quantity <= 0;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}