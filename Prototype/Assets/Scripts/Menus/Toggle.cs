using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour
{
    public void ToggleActive()
    {
        // +++++++++ IMPLEMENTATION WITH FloatEditor OBJECTS +++++++++
        // Write the volumes of the Settings object into the FloatEditor members of our Slider objects
        GetComponent<SettingsUI>().musicVolume.floatValue = GetComponent<SettingsUI>().settings.musicVolume;
        GetComponent<SettingsUI>().fxVolume.floatValue = GetComponent<SettingsUI>().settings.soundFxVolume;


        //// ********* IMPLEMENTATION WITH SLIDER OBJECTS ********* 
        //// Write the volume settings of the Settings object into the .value members of our Slider objects
        //GetComponent<SettingsUI>().musicVolumeSlider.value = GetComponent<SettingsUI>().settings.musicVolume;
        //GetComponent<SettingsUI>().soundFxVolumeSlider.value = GetComponent<SettingsUI>().settings.soundFxVolume;

        // These are Unity keywords, functions and keyword member variables
        gameObject.SetActive(!gameObject.activeSelf);
        // gameObject is like a 'this' call
        // activeSelf is a parameter of (presumably) every gameObject
        // SetActive is obvious
    }
}
