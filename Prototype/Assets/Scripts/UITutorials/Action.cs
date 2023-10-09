using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A class for a single type of action
public class Action : MonoBehaviour
{
    public string actionName;
    public string description;

    // An image for use in the UI
    public Sprite icon;

    // A colour for differentiating the Sprite
    public Color colour = Color.white;
}
