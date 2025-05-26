using TMPro;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    public TMP_Text letterText;

    public void SetLetter(string letter)
    {
        letterText.text = letter;
    }

    public string GetLetter()
    {
        return letterText.text;
    }
}
