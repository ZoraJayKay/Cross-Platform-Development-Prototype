using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    // ----- InventoryUI member variables  -----
    // 1: A back-end Inventory to create a UI for.
    public Inventory inventory;
    // 2: The base Slot prefab we want to use for this InventoryUI.
    public Slot slotPrefab;
    // 3: An array of Slots to hold ShopItemUI's.
    Slot[] slots;
    // 4: The base ShopItemUI prefab we want to use for this InventoryUI.
    public ShopItemUI shopItemUIPrefab;
    // 5: Variables for the components of this InventoryUI
    LayoutGroup layoutGroup;
    ContentSizeFitter contentSizeFitter;
    
    

    // Initialise the InventoryUI
    private void Start()
    {        
        // 1: Set the variables for the components
        layoutGroup = GetComponent<LayoutGroup>();
        contentSizeFitter = GetComponent<ContentSizeFitter>();

        // 2: Update once on startup
        StartCoroutine(UpdateUI());

        // 3: Subscribe the Inventory of this InventoryUI to the event that will notify the UI of changes to the underlying Inventory
        //inventory.onChanged.AddListener(() => { StartCoroutine(UpdateUI()); });

        StopCoroutine(UpdateUI());
    }

    // Update the InventoryUI, returning an iterator over the Inventory of this InventoryUI
    public IEnumerator UpdateUI()
    {
        // 1: Turn on the components
        contentSizeFitter.enabled = true;
        layoutGroup.enabled = true;

        // 2: Wait until the end of the frame and then continue
        yield return new WaitForEndOfFrame();

        // 4: Create a Slot in our array for each ShopItem in the passed-in Inventory.
        //slots = new Slot[inventorySize];
        slots = new Slot[inventory.shopItems.Length];

        // 5: Initialise the array of Slots.
        // 5.1: Iterate through the Slots...
        for (int i = 0; i < inventory.shopItems.Length; i++)
        {
            // 5.1a: For each Slot, create a Slot prefab according to the transform rules of the InventoryUI.
            slots[i] = Instantiate(slotPrefab, transform);

            // 5.1b: For each Slot, create a ShopItemUI prefab as the Slot's own child
            slots[i].shopItemUI = Instantiate(shopItemUIPrefab, slots[i].transform);

            // 5.1c: Assign the Slot's ShopItemUI with the particulars of its ShopItem
            slots[i].shopItemUI.SetItem(inventory.shopItems[i]);

            // 5.1d: Initialise the Slot with the particulars of the ShopItemUI
            slots[i].Init(this, i, slots[i].shopItemUI);
        }

        // 6: Wait appropriate frames (yield return only returns those items required / calculated by a function, so that function will exit early if it would otherwise calculate more than needed).
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // 7: Turn off the content size fitter component and layout group component
        contentSizeFitter.enabled = false;
        layoutGroup.enabled = false;

        gameObject.SetActive(false);
    }

    public ref Slot GetSlot(int i)
    {
        return ref slots[i];
    }
}
