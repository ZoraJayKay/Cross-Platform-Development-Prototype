using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// A front-end class for the empty Slots which sit in the Inventory, with or without items inside them.
public class Slot : MonoBehaviour
{
    // The specific ShopItemUI object for this Slot
    [HideInInspector]
    public ShopItemUI shopItemUI;

    //// A second ShopItemUI reference exclusively for the purposes of drag and drop item swapping
    //[HideInInspector]
    //public ShopItemUI tempItemUI;

    [HideInInspector]
    public InventoryUI inventoryUI;
    
    [HideInInspector]
    public int arrayIndex;

    public TextMeshProUGUI indexNo;

    // A function for initialising the Slots of an InventoryUI with the particulars of its ShopItemUI; link up references between the Slot, the ShopItemUI, the InventoryUI, and the Slot's index in the array
    public void Init(InventoryUI invUI, int i, ShopItemUI _shopItemUI)
    {
        // Store a reference to the ShopItemUI that's passed in (because it is the one for this Slot)
        shopItemUI = _shopItemUI;
        inventoryUI = invUI;
        arrayIndex = i;
        shopItemUI._slot = this;
        shopItemUI._originalParent = this.transform;
        indexNo.SetText(arrayIndex.ToString());
    }

    public void UpdateItem(ShopItem item)
    {
        // Update the raw data in the inventory
        inventoryUI.inventory.shopItems[arrayIndex] = item;

        // Update the UI
        shopItemUI.SetItem(item);
    }
}
