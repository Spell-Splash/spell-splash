using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseMenuPanel;

    private bool isPaused = false;

    void Start()
    {
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void ReturnToGame()
    {
        isPaused = false;
        pauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void GoToSettings()
    {
        Time.timeScale = 1f; // Resume in case it's still paused
        SceneManager.LoadScene("Setting");
    }

    public void QuitToGuild()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Guild");
    }
}
