using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void DoAction(Action action) 
    {
        // Print the name of the action being invoked to the console
        Debug.Log("Doing " + action.actionName);
    }
}
