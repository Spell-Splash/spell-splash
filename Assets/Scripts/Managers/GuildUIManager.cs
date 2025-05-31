using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GuildUIManager : MonoBehaviour
{
    // Called when a "Battle" button is clicked
    public void OnBattleButtonClicked()
    {
        SceneManager.LoadScene("BattleScene");
    }
}
