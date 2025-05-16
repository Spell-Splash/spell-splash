using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    // Called when Play button is clicked
    public void OnPlayButtonClicked()
    {
        // #TODO: Implement logic to load last visited scene instead of hardcoding
        SceneManager.LoadScene("MapScene");
    }

    // Called when Settings button is clicked
    public void OnSettingsButtonClicked()
    {
        SceneManager.LoadScene("Settings");
    }

    // Called when Quit button is clicked
    public void OnQuitButtonClicked()
    {
        Application.Quit();

#if UNITY_EDITOR
        // Stop play mode if running inside the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
