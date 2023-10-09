using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// The ActionUI is the front-end UI which take an Action and shows it to the player.
public class ActionUI : MonoBehaviour
{
    // Receive an Action object
    public Action action;

    // Create a header in the public component viewer 
    [Header("Child Components")]
    // Receive an Image object
    public Image icon;
    // Receive a TMP object
    public TextMeshProUGUI nameTag;
    // Receive a TMP object
    public TextMeshProUGUI descriptionTag;

    // Start is called before the first frame update (only for this function)
    private void Start()
    {
        // Set the Action object passed into the component viewer
        if (action) { SetAction(action); }
    }

    public void SetAction(Action a)
    {
        action = a;

        if (nameTag)
        {
            nameTag.text = action.actionName;
        };
        if (descriptionTag)
        {
            descriptionTag.text = action.description;
        };
        if (icon)
        {
            icon.sprite = action.icon;
            icon.color = action.colour;
        };
    }

    // The specific Player object
    Player player;

    // Add a lambda function to the Player's onClick button function
    public void Init(Player _player)
    {
        // Store a reference to the player that uses this function
        player = _player;
        
        // Store a reference to the button of this Action (Init is being called in a loop so this will get done for all of the Actions)
        Button button = GetComponentInChildren<Button>();

        // The lambda function here creates a local unnamed function that gets called when the button is pressed, with a copy of the stack at the time it was set up.
        if (button)
        {
            button.onClick.AddListener(() => { player.DoAction(action); });
        }
    }
}
