using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class FloatEditor : MonoBehaviour
{
    [Header("Components")]
    public Slider slider;
    public TMP_InputField input;
    public string formatString = "0";

    // A property for a float value that sets any slider or input we may have attached
    float _floatValue;

    void Start()
    {
        if (slider)
        {
            slider.onValueChanged.AddListener((float value) => { floatValue = value; });
        }

        if (input)
        {
            input.onEndEdit.AddListener((string text) =>
            {
                float parsedValue;
                if (float.TryParse(text, out parsedValue))
                {
                    floatValue = parsedValue;
                }
            });
        }
    }

    // A public float that we can use to set the back-end value of the slider or input
    public float floatValue
    {
        get { return _floatValue; }

        set 
        { 
            // Update the back-end variable
            _floatValue = value; 

            // Make sure all the controls are visually consistent
            if (slider)
            {
                slider.value = value;
            }

            if (input)
            {
                input.text = value.ToString(formatString);
            }

            // Update any client code that has registered with this event
            onValueChanged.Invoke(_floatValue);
        }
    }

    [System.Serializable]
    public class FloatEvent : UnityEvent<float> { }

    // This creates a customisable Unity event for this script which we can edit in the Inspector
    public FloatEvent onValueChanged;
}
