using UnityEngine;

[CreateAssetMenu(fileName = "WordDatabase", menuName = "Game/Word Database")]
public class WordDatabase : ScriptableObject
{
    public WordEntry[] words;
}

[System.Serializable]
public class WordEntry
{
    public string word;       // EN
    public string definition; // TH
}
