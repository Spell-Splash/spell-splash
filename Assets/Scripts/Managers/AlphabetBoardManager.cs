using System.Collections.Generic;
using UnityEngine;

public class AlphabetBoardManager : MonoBehaviour
{
    public List<AlphabetButton> allAlphabetButtons; // Drag all buttons A–Z here

    public int totalLettersToDistribute = 10;

    void Start()
    {
        DistributeRandomLetters();
    }

    public void DistributeRandomLetters()
    {
        // Reset all first
        foreach (var btn in allAlphabetButtons)
        {
            btn.SetAmount(0);
        }

        // Randomly assign letters
        for (int i = 0; i < totalLettersToDistribute; i++)
        {
            int index = Random.Range(0, allAlphabetButtons.Count);
            allAlphabetButtons[index].SetAmount(allAlphabetButtons[index].amount + 1);
        }
    }
}
