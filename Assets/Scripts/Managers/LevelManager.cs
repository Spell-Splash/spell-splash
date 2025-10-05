using UnityEngine;
using UnityEngine.SceneManagement;



[System.Serializable]
public class WaveData
{
    public string waveName;
    public int enemyMaxHP;
    public int playerMaxHP;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public WordBuilder wordBuilder;
    public WordQuizManager wordQuizManager;


    public HealthBarController playerHPBar;
    public HealthBarController enemyHPBar;

    public int playerHP = 100;
    public int enemyHP = 100;

    public bool isPlayerTurn = true;
    public bool ActionUsedThisTurn = false;
    private bool isBattleOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        StartBattle(); // Load wave data here later
        playerHP = 100;
        enemyHP = 100;
        playerHPBar.Initialize(playerHP);
        enemyHPBar.Initialize(enemyHP);
    }

    void StartBattle()
    {
        isPlayerTurn = true;
        isBattleOver = false;
        // TODO: Load wave-specific HP/stats

        wordQuizManager.GenerateQuestion();
    }

    public void EndTurn()
    {
        if (isBattleOver) return;

        isPlayerTurn = !isPlayerTurn;

        Debug.Log($"Turn changed to: {(isPlayerTurn ? "Player" : "Enemy")}");

        if (!isPlayerTurn)
        {
            Invoke(nameof(EnemyTurn), 1f); // Delay for enemy "thinking"
            wordQuizManager.GenerateQuestion();
        }
    }

    void EnemyTurn()
    {
        if (Random.value > 0.0f)
        {
            DirectHit();
        }
        else
        {
            CursedHit();
        }

        CheckBattleEnd();
        if (!isBattleOver)
            isPlayerTurn = true;
    }

    void DirectHit()
    {
        int damage = Random.Range(1, 10);
        playerHP -= damage;
        Debug.Log("Enemy hits you for " + damage);

        playerHPBar.SetHP(playerHP);
    }

    void CursedHit()
    {
        Debug.Log("Enemy sends you to cursed scene!");
        SceneManager.LoadScene("CursedScene");
    }

    public void PlayerAttack()
    {
        string word = wordBuilder.GetBuiltWord();
        int damage = word.Length * 2;

        Debug.Log($"You attacked with \"{word}\" for {damage} damage.");

        enemyHP -= damage;
        enemyHPBar.SetHP(enemyHP);

        CheckBattleEnd();
        EndTurn();
    }


    void CheckBattleEnd()
    {
        if (playerHP <= 0)
        {
            isBattleOver = true;
            Debug.Log("You Died!");
            SceneManager.LoadScene("Guild");
        }
        else if (enemyHP <= 0)
        {
            isBattleOver = true;
            Debug.Log("Enemy Defeated!");
            SceneManager.LoadScene("Guild");
        }
    }
}
