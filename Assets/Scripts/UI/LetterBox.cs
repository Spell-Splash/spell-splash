using TMPro;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    public TMP_Text letterText;
    private string storedLetter;

    public void SetLetter(string letter)
    {
        storedLetter = letter;
        letterText.text = letter;
    }

    public string GetLetter()
    {
        return storedLetter;
    }
}
