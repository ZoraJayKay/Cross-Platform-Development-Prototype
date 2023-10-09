using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This could also be set up as a ScriptableObject
public class Settings : MonoBehaviour
{
    public float musicVolume;
    public float soundFxVolume;

    public bool stereoIsOn = true;

    // A function to tell the stereo Toggle in the SettingsUI whether it has been turned on or off
    public void OnToggleChange(bool checkbox)
    {
        stereoIsOn = checkbox;
    }
}
