using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordQuizUI : MonoBehaviour
{
    [Header("Right panel")]
    public TMP_Text titleLabel;      // the big “Word” title (optional)
    public TMP_Text definitionLabel; // shows Thai definition
    public Button[] optionButtons;   // exactly 3 buttons
    public TMP_Text[] optionLabels;  // text on each button

    public void SetDefinition(string thai)
    {
        definitionLabel.text = thai;
    }

    public void SetOptions(string[] englishOptions)
    {
        for (int i = 0; i < optionLabels.Length; i++)
            optionLabels[i].text = englishOptions[i];
    }

    public void SetInteractable(bool value)
    {
        foreach (var b in optionButtons) b.interactable = value;
    }
}
