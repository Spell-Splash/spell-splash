using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;  // Required for TMP_Text
using UnityEngine.UI; // Required for Button

public class NewBehaviourScript : MonoBehaviour
{
    private int clickCount = 0;

    void Start()
    {
        Debug.Log("Script started!");
    }

    // Call this from each button and pass its GameObject as parameter
    public void OnChoiceButtonClick(GameObject buttonObject)
    {
        clickCount++;

        // Get TMP_Text from the child of the button (assumes it's the first TMP_Text)
        TMP_Text textComponent = buttonObject.GetComponentInChildren<TMP_Text>();
        if (textComponent != null)
        {
            Debug.Log($"Button {clickCount} clicked with text: {textComponent.text}");
        }
        else
        {
            Debug.LogWarning("TMP_Text component not found on button!");
        }
    }
}
