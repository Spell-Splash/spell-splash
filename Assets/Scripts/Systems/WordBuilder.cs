using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WordBuilder : MonoBehaviour
{
    public static WordBuilder Instance;

    [Header("UI References")]
    public Transform wordPanel; // The gray box (with HorizontalLayoutGroup)
    public GameObject letterBoxPrefab;

    // Track added letters & original AlphabetButtons
    private List<GameObject> currentLetters = new List<GameObject>();
    private List<AlphabetButton> usedButtons = new List<AlphabetButton>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void TryAddLetterFromButton(AlphabetButton button)
    {
        if (button.TryUseLetter())
        {
            GameObject letterObj = Instantiate(letterBoxPrefab, wordPanel);
            LetterBox box = letterObj.GetComponent<LetterBox>();
            box.SetLetter(button.letter);

            currentLetters.Add(letterObj);
            usedButtons.Add(button);
        }
    }

    public void DeleteLastLetter()
    {
        if (currentLetters.Count == 0)
            return;

        // Remove from UI
        GameObject lastLetter = currentLetters[currentLetters.Count - 1];
        Destroy(lastLetter);
        currentLetters.RemoveAt(currentLetters.Count - 1);

        // Restore amount to source alphabet button
        AlphabetButton lastUsed = usedButtons[usedButtons.Count - 1];
        lastUsed.SetAmount(lastUsed.amount + 1);
        usedButtons.RemoveAt(usedButtons.Count - 1);
    }

    public void ClearAll()
    {
        while (currentLetters.Count > 0)
        {
            DeleteLastLetter();
        }
    }

    public string GetBuiltWord()
    {
        string word = "";
        foreach (var obj in currentLetters)
        {
            word += obj.GetComponent<LetterBox>().GetLetter();
        }
        return word;
    }
}
