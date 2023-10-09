using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

// This is necessary to create custom events
using UnityEngine.Events;

public class ActionList : MonoBehaviour
{
    // An event to detect additions or removals to the ActionList
    public UnityEvent onChanged;

    // An array of all sibling Actions (empty on instantiation)
    Action[] _actions = null;

    // A 'lazy' initialisation accessor INSTEAD OF a Start()
    public Action[] actions
    {
        get
        {
            if (_actions == null)
                // Return all sibling Actions to the array
                _actions = GetComponents<Action>();
                return _actions;
        }
    }

    // A button in the ... menu of this Script to access this function
    [ContextMenu("Delete First")]
    void DeleteFirst()
    {
        // A temporary list of actions based on the existing one
        List<Action> actions = new List<Action>(_actions);
        // Delete the first entry
        actions.RemoveAt(0);
        
        // Set the array of all Actions equal to the revised list that has just had the front entry removed
        _actions = actions.ToArray();

        // Invoke our custom event
        onChanged.Invoke();
    }

    // A button in the ... menu of this Script to access this function
    [ContextMenu("Duplicate Random")]
    void DuplicateRandom()
    {
        // Return how many elements there are
        int i = _actions.Length;

        if (i < 8)
        {
            // Choose one of them at random
            int randomNumber = Random.Range(0, i);

            // A temporary list of actions based on the existing one
            List<Action> actions = new List<Action>(_actions);

            //
            actions.Add(_actions[randomNumber]);

            // Set the array of all Actions equal to the revised list that has just had the front entry removed
            _actions = actions.ToArray();

            // Invoke our custom event
            onChanged.Invoke();
        }        
    }
}
