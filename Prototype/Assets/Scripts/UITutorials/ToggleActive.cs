using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleActive : MonoBehaviour
{
    public void Toggle()
    {
        // These are Unity keywords, functions and keyword member variables
        gameObject.SetActive(!gameObject.activeSelf);
        // gameObject is like a 'this' call
        // activeSelf is a parameter of (presumably) every gameObject
        // SetActive is obvious
    }
}
