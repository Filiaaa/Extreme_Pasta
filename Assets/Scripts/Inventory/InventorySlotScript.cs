using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotScript : MonoBehaviour
{
    public InventorySys inventorySys;
    public int slotnumb;

    public void Click()
    {

        inventorySys.UseItemFromSlot(slotnumb);
    }
}
