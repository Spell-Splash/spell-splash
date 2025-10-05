using System.Collections.Generic;
using UnityEngine;

public class WordQuizManager : MonoBehaviour
{
    public WordDatabase database;
    public WordQuizUI ui;
    public InventoryManager inventory;

    private int correctIndex = -1;       // 0..2 in current options
    private string[] currentOptions = new string[3];
    private WordEntry current;           // current correct entry

    // call this at the START of player's turn
    public void GenerateQuestion()
    {
        Debug.Log("Starting new question generation.");
        if (database == null || database.words == null || database.words.Length < 3)
        {
            Debug.LogWarning("Need at least 3 word entries.");
            ui.SetDefinition("—");
            ui.SetOptions(new[] { "-", "-", "-" });
            ui.SetInteractable(false);
            return;
        }

        // 1) pick correct
        current = database.words[Random.Range(0, database.words.Length)];

        // 2) pick two distractors (unique)
        var used = new HashSet<int>();
        var distractors = new List<string>();
        while (distractors.Count < 2)
        {
            int idx = Random.Range(0, database.words.Length);
            if (database.words[idx].word == current.word) continue;
            if (used.Add(idx)) distractors.Add(database.words[idx].word);
        }

        // 3) compose options and shuffle
        currentOptions[0] = current.word;
        currentOptions[1] = distractors[0];
        currentOptions[2] = distractors[1];
        Shuffle(currentOptions);

        // 4) find correct index
        for (int i = 0; i < 3; i++)
            if (currentOptions[i] == current.word) correctIndex = i;

        // 5) push to UI
        ui.SetDefinition(current.definition);    // Thai text
        ui.SetOptions(currentOptions);
        ui.SetInteractable(true);
    }

    // wired to each button’s OnClick with parameter 0/1/2
    public void OnOptionClicked(int i)
    {
        if (!LevelManager.Instance.isPlayerTurn) return;          // safety
        if (LevelManager.Instance.ActionUsedThisTurn) return;     // one action only

        // LevelManager.Instance.ActionUsedThisTurn = true;          // lock in C chosen
        ui.SetInteractable(false);

        bool correct = (i == correctIndex);
        if (correct)
        {
            Debug.Log("✅ Correct! Rewarding item.");
            inventory.TryAddRandomItem(); // ignore full case or show toast
        }
        else
        {
            Debug.Log("❌ Wrong. No reward this time.");
        }

        // End the player's turn after feedback (can add small delay/FX)
        LevelManager.Instance.EndTurn();
    }

    // Fisher–Yates
    private void Shuffle(string[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (arr[i], arr[j]) = (arr[j], arr[i]);
        }
    }
}
