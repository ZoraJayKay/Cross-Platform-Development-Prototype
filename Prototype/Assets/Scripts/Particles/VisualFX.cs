using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// A scriptable object back-end class that specifies the particulars for each visual effect
// Right click > Create > VisualFXSystem > VisualFX
[CreateAssetMenu(fileName ="VisualFX", menuName ="VisualFXSystem/VisualFX")]
public class VisualFX : ScriptableObject
{
    // +++ Public variables for setting in Unity +++
    // 1:The visual effect prefab we want to clone
    public GameObject prefab;

    // 2: Conditional spawn options
    public bool attach;
    public bool orient;

    // 3: 
    public GameObject Spawn(Transform t)
    {
        // What is the int for?


        // Do we want the visual effect to be childed to the parent and follow it, or simply occur at its location?
        // Create a Transform which is the same as a passed-in Transform if the 'attach' bool is switched on, or null if it is switched off
        Transform parent = attach ? t : null;

        // Do we want to match the orientation of the spawn point or not?
        // Create a quaternion which is the same as the rotation of the passed-in Transform if the 'orient' bool is switched on, or the identity quaternion if it is switched off
        Quaternion orientation = orient ? t.rotation : Quaternion.identity;

        // Create a copy of the prefab at the spawnpoint with the desired settings
        return Instantiate(prefab, t.position, orientation, parent);
    }
}
