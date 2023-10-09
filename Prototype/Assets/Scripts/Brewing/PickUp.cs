using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickUp : MonoBehaviour //, IPointerDownHandler
{
    public GameObject player;
    public Inventory playerInventory;
    public GameObject playerInventoryObject;
    public InventoryUI ui;
    public AudioSource pickupAudioSource;

    // The type of item that will be picked up when the player clicks on this pickup
    public ShopItem item;


    private void Start()
    {
        pickupAudioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<BackgroundMusic>().pickupSound;
        // Get a reference to the player
        player = GameObject.FindGameObjectWithTag("Player");
        // Get a reference to that player's inventory so we know where to put this item when they pick it up
        playerInventory = player.GetComponentInChildren<Inventory>();
        //
        playerInventoryObject = GameObject.FindGameObjectWithTag("PlayerInventory");
        //
        ui = playerInventoryObject.GetComponent<InventoryUI>();
    }


    public void AddToInventory()
    {
        // Look through the player's inventory to make sure there's room to place this item
        for (int i = 0; i < playerInventory.shopItems.Length; i++)
        {
            // If the inventory has a free space...
            if (playerInventory.shopItems[i] == null)
            {
                ui.GetSlot(i).UpdateItem(item);
                Destroy(this.gameObject);
                pickupAudioSource.Play();
                break;
            }

            else { continue; }
        }
    }
}
