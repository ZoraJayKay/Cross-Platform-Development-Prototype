using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleVisibility : MonoBehaviour
{
    public bool isVisible = true;

    public void Toggle()
    {
        // These are Unity keywords, functions and keyword member variables
        gameObject.SetActive(!gameObject.activeSelf);
        // gameObject is like a 'this' call
        // activeSelf is a parameter of (presumably) every gameObject
        // SetActive is obvious
    }

    // A function to tell the visibility panel Toggle in the Panel whether it has been turned on or off
    public void OnToggleChange(bool checkbox)
    {
        isVisible = checkbox;
    }
}
