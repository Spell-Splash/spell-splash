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

    // NEW: keep max for heals (for now, mirror initial values)
    public int playerMaxHP = 100;
    public int enemyMaxHP = 100;

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

        // (for now) initialise maxes to starting HP
        playerMaxHP = playerHP = 100;
        enemyMaxHP  = enemyHP  = 100;

        playerHPBar.Initialize(playerHP);
        enemyHPBar.Initialize(enemyHP);

        // Start with a fresh player turn & question
        StartPlayerTurn();
    }

    void StartBattle()
    {
        isPlayerTurn = true;
        isBattleOver = false;
        // TODO: Load wave-specific HP/stats
    }

    // NEW: centralised start-of-player-turn
    void StartPlayerTurn()
    {
        ActionUsedThisTurn = false;
        if (wordQuizManager != null)
            wordQuizManager.GenerateQuestion();
    }

    public void EndTurn()
    {
        if (isBattleOver) return;

        isPlayerTurn = !isPlayerTurn;
        Debug.Log($"Turn changed to: {(isPlayerTurn ? "Player" : "Enemy")}");

        if (!isPlayerTurn)
        {
            // enemy "thinking" delay
            Invoke(nameof(EnemyTurn), 1f);
        }
        else
        {
            // when it becomes player turn again
            StartPlayerTurn();
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
        {
            isPlayerTurn = true;
            StartPlayerTurn(); // get the next question ready
        }
    }

    void DirectHit()
    {
        int damage = Random.Range(1, 10);
        playerHP -= damage;
        playerHP = Mathf.Max(0, playerHP);
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
        enemyHP = Mathf.Max(0, enemyHP);
        enemyHPBar.SetHP(enemyHP);

        CheckBattleEnd();
        ActionUsedThisTurn = true;   // counts as an action
        EndTurn();
    }

    // ---------- NEW: simple guard for player actions ----------
    bool CanAct()
    {
        return isPlayerTurn && !ActionUsedThisTurn && !isBattleOver;
    }

    // ---------- NEW ACTION 1: Heal player to full ----------
    public void Action_FullHeal()
    {
        if (!CanAct()) return;

        playerHP = playerMaxHP;
        playerHPBar.SetHP(playerHP);
        Debug.Log("You cast Heal. HP restored to full.");

        ActionUsedThisTurn = true;
        CheckBattleEnd();
        EndTurn();
    }

    // ---------- NEW ACTION 2: Deal flat 20 to enemy ----------
    public void Action_HitEnemy20()
    {
        if (!CanAct()) return;

        enemyHP = Mathf.Max(0, enemyHP - 20);
        enemyHPBar.SetHP(enemyHP);
        Debug.Log("You cast Firebolt. Enemy took 20 damage!");

        ActionUsedThisTurn = true;
        CheckBattleEnd();
        EndTurn();
    }

    // ---------- NEW ACTION 3: Get a random word (refresh quiz) ----------
    public void Action_GetRandomWord()
    {
        if (!CanAct()) return;

        if (wordQuizManager != null)
            wordQuizManager.GenerateQuestion(); // reuse your quiz UI

        Debug.Log("You conjured a new word question.");
        ActionUsedThisTurn = true;
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
