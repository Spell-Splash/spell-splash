using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class WordBuilder : MonoBehaviour
{
    public static WordBuilder Instance;

    [Header("UI References")]
    public Transform wordPanel; // The gray box (with HorizontalLayoutGroup)
    public GameObject letterBoxPrefab;
    public Button attackButton;

    // Track added letters & original AlphabetButtons
    private List<GameObject> currentLetters = new List<GameObject>();
    private List<AlphabetButton> usedButtons = new List<AlphabetButton>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        attackButton.interactable = false;
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

            attackButton.interactable = true;
        }
    }

    public void ClearAll(bool refundLetters = true)
    {
        while (currentLetters.Count > 0)
        {
            DeleteLastLetter(refundLetters);
        }
    }

    public void DeleteLastLetter(bool refund = true)
    {
        if (currentLetters.Count == 0)
            return;

        GameObject lastLetter = currentLetters[^1];
        Destroy(lastLetter);
        currentLetters.RemoveAt(currentLetters.Count - 1);

        AlphabetButton lastUsed = usedButtons[^1];

        if (refund)
        {
            lastUsed.SetAmount(lastUsed.amount + 1);
        }

        usedButtons.RemoveAt(usedButtons.Count - 1);

        if (currentLetters.Count == 0)
        {
            attackButton.interactable = false;
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

    public void OnAttackPressed()
    {
        string word = GetBuiltWord();

        Debug.Log("Player attacks with: " + word);

        // TODO: Add actual battle logic here
        // For now: just clear without refunding letters
        ClearAll(refundLetters: false);

        attackButton.interactable = false;
    }

}
