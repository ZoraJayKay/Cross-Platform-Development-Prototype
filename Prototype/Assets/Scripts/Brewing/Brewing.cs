using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brewing : MonoBehaviour
{    
    public Cookbook cookbook;
    public Inventory outputInventory;
    public GameObject outputInventoryPalette;
    public InventoryUI targetUI;
    private AudioSource successfulBrew;

    public void Start()
    {
        successfulBrew = GameObject.FindGameObjectWithTag("Music").GetComponent<BackgroundMusic>().potionCompleteSound;
    }

    public void BrewPotion()
    {
        outputInventoryPalette.SetActive(true);
        
        Inventory inv = GetComponent<InventoryUI>().inventory;
        InventoryUI ui = GetComponent<InventoryUI>();

        for (int i = 0; i < cookbook.recipes.Length; i++)
        {
            if (inv.shopItems[0] == cookbook.recipes[i].ingredient01 &&
                inv.shopItems[1] == cookbook.recipes[i].ingredient02 &&
                inv.shopItems[2] == cookbook.recipes[i].ingredient03)
            {
                // 'Delete' all of the items in the 'cauldron' and if there's anything in the UI inventory
                inv.shopItems[0] = null;
                inv.shopItems[1] = null;
                inv.shopItems[2] = null;                

                // Update the UIs of the 'deleted' items
                ui.GetSlot(0).UpdateItem(inv.shopItems[0]);
                ui.GetSlot(1).UpdateItem(inv.shopItems[1]);
                ui.GetSlot(2).UpdateItem(inv.shopItems[2]);

                targetUI.GetSlot(0).UpdateItem(cookbook.recipes[i].potion);

                outputInventoryPalette.SetActive(true);
                targetUI.gameObject.SetActive(true);

                successfulBrew.Play();
            }
        }
    }
}
