using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// This for the click and drag funtionality
using UnityEngine.EventSystems;

// This so that I can make generic reference to ShopItems that aren't specific instances
using static ShopItem;

// Border frame asset from Kenney
// https://www.kenney.nl/assets/generic-items


// Have the ShopItemUI inherit 3 Unity classes for drag and drop, each of which requires a function within this class
public class ShopItemUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // ************ Member variables ************
    
    

    // Create a header in the public component viewer 
    [Header("Child Components")]
    // +++ Public variables for setting in Unity +++
    // 1: An image for use by this object's ShopItemUI
    public Image _icon;

    // 2: Text strings
    public TextMeshProUGUI _itemName;
    public TextMeshProUGUI _itemDescription;
    public TextMeshProUGUI _indexNo;

    // 3: Requirements to equip this item
    public TextMeshProUGUI _itemPrice;
    public TextMeshProUGUI _itemWeight;
    // The user would have to be this class to equip the item
    public TextMeshProUGUI _classTypeTag;
    ShopItem.classRequired _thisItemClass;

    // 4: Other types of constraints
    public TextMeshProUGUI _itemTypeTag;
    ShopItem.itemType _thisItemType;    

    // 5: The parent Transform and parent Canvas
    [Header("Related Non-child Components")]
    // The transform of the parent Slot
    public Transform _originalParent;
    // The Canvas of the whole scene
    public Canvas _canvas;
    // The parent Slot of this ShopItemUI
    public Slot _slot;
    // The ShopItem of this ShopItemUI
    public ShopItem _shopItem;

    // 6: Drag and drop variables
    bool dragging = false;

    // 7: Two sounds, one for on click, one for release of drag and drop
    AudioSource soundOnCommenceDrag;
    AudioSource soundOnEndDrag;


    // ************  ************
    private void Start()
    {
        // Cache the sounds I want to use
        soundOnCommenceDrag = GameObject.FindGameObjectWithTag("Music").GetComponent<BackgroundMusic>().dragCommenceSound;
        soundOnEndDrag = GameObject.FindGameObjectWithTag("Music").GetComponent<BackgroundMusic>().dropItemSound;
    }


    // Make the ShopItemUI moveable with click and drag
    // 1: On mouse button down...
    public void OnBeginDrag(PointerEventData eventData)
    {
        soundOnCommenceDrag.Play();

        // Cache the original Parent's Transform
        if (_originalParent == null){
            _originalParent = transform.parent;
        }

        // Find the overall parent Canvas
        if (_canvas == null){
            _canvas = GetComponentInParent<Canvas>();
        }

        // Make this ShopItemUI the most visible object in the scene by making it the child of the Canvas for the duration of the drag
        transform.SetParent(_canvas.transform, true);
        transform.SetAsLastSibling();

        // Set dragging indicator to true
        dragging = true;
    }

    // 2: For the duration of the drag...
    public void OnDrag(PointerEventData eventData) 
    { 
        // While dragging, make the position of the ShopItemUI equal to the position of the cursor
        if (dragging)
        {
            transform.position = eventData.position;
        }
    }

    // 3: On mouse button up, see if there�s a Slot under the mouse using EventSystem.RaycastAll.
    // Make a list of the results from checking under the cursor
    List<RaycastResult> hits = new List<RaycastResult>();

    public void OnEndDrag(PointerEventData eventData)
    {
        // Is there a slot underneath the cursor? Start with a null result by default
        Slot slotFound = null;

        // Keep a list of the things we mouse over 
        EventSystem.current.RaycastAll(eventData, hits);

        // For everything we mouse over...
        foreach (RaycastResult hit in hits)
        {
            // Check whether the thing we're mousing over has a Slot
            Slot s = hit.gameObject.GetComponent<Slot>();
            if (s)
            {
                // FROM TUTE AND EXAMPLE    
                // If yes, keep a reference to that Slot that was under the mouse
                slotFound = s;

#if DEBUG
                Debug.Log("FOUND A SLOT UNDER THE MOUSE");
#endif

                // Swap the underlying ShopItems of this ShopItemUI and the 
                Swap(slotFound);

                transform.SetParent(_originalParent);
                transform.localPosition = Vector3.zero;

                soundOnEndDrag.Play();
#if DEBUG
                Debug.Log("position: " + transform.localPosition);
#endif
            }

            else if (!s)
            {
                Debug.Log("NO SLOT FOUND UNDER THE MOUSE");

                transform.SetParent(_originalParent);

                transform.localPosition = Vector3.zero;
            }
        }

        dragging = false;
    }


    // A function for assigning a ShopItemUI with the particulars of a ShopItem
    public void SetItem(ShopItem i)
    {
        // Give this ShopItemUI a reference to its ShopItem 
        _shopItem = i;

        if (_shopItem)
        {
            // Bring in the sprite and its colour if they exist
            if (_icon)
            {
                _icon.sprite = _shopItem.icon;
                _icon.color = _shopItem.colour;
            }

            // Bring in the strings if they exist
            if (_itemName)
            {
                _itemName.SetText(_shopItem.itemName.ToString());
            }

            if (_itemDescription)
            {
                _itemDescription.SetText(_shopItem.itemDescription.ToString());
            }   
            

            //SetItemType(_shopItem.GetItemType());
            //SetClassType(_shopItem.GetClassRequired());

            //// Set the constraints
            //_itemPrice.SetText("Price: " + _shopItem.itemPrice.ToString());
            //_itemWeight.SetText("Weight: " + _shopItem.itemWeight.ToString());

            //// Set the other constraints
            //_itemTypeTag.SetText(_thisItemType.ToString());
            //_classTypeTag.SetText("Class: " + _thisItemClass.ToString());
        }
        
        gameObject.SetActive(_shopItem != null);
    }

    // Assign the ShopItemUI an item type according to its ShopItem
    private void SetItemType(int itemType)
    {
        switch(itemType)
        {
            case 1: 
                _thisItemType = ShopItem.itemType.Weapon; 
                break;
            case 2:
                _thisItemType = ShopItem.itemType.Armour;
                break;
            case 3:
                _thisItemType = ShopItem.itemType.Consumable;
                break;
        }
    }

    // Assign the ShopItemUI a required class according to its ShopItem
    private void SetClassType(int classRequired)
    {
        switch (classRequired)
        {
            case 1:
                _thisItemClass = ShopItem.classRequired.Warrior;
                break;
            case 2:
                _thisItemClass = ShopItem.classRequired.Mage;
                break;
            case 3:
                _thisItemClass = ShopItem.classRequired.Cleric;
                break;
            case 4:
                _thisItemClass = ShopItem.classRequired.All;
                break;
        }
    }

    protected void Swap(Slot newParent)
    {
        ShopItemUI otherUI = newParent.shopItemUI as ShopItemUI;

        Debug.Log("Swap started");

        if (otherUI)
        {
            Debug.Log("If was true");

            ShopItem ourItem = _shopItem;
            ShopItem theirItem = otherUI._shopItem;



            _slot.UpdateItem(theirItem);
            otherUI._slot.UpdateItem(ourItem);
        }
    }
}
