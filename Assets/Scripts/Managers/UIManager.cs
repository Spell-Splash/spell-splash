using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

[System.Serializable]
public class QuizResponse
{
    public string correct;
    public string[] choices;
    public string audio_file;
}

public class UIManager : MonoBehaviour
{
    public GameObject[] choiceButtons;
    public TMP_Text feedbackText;
    public Button restartButton;
    public Button replayButton;
    public AudioSource audioSource;

    private string correctAnswer;
    private GameState currentState;

    enum GameState
    {
        Start,
        Select,
        Decision,
        Result
    }

    void Start()
    {
        restartButton.onClick.AddListener(RestartGame);
        replayButton.onClick.AddListener(OnReplayAudio);
        SetState(GameState.Start);
    }

    void SetState(GameState newState)
    {
        currentState = newState;

        switch (newState)
        {
            case GameState.Start:
                feedbackText.text = "";
                restartButton.gameObject.SetActive(false);
                StartCoroutine(LoadQuestionFromServer());
                break;

            case GameState.Select:
                // Wait for user input
                break;

            case GameState.Decision:
                // Handled in OnChoiceClicked
                break;

            case GameState.Result:
                restartButton.gameObject.SetActive(true);
                break;
        }
    }

    IEnumerator LoadQuestionFromServer()
    {
        UnityWebRequest request = UnityWebRequest.Get("http://127.0.0.1:8000/generate");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            feedbackText.text = "Failed to load question.";
            Debug.LogError(request.error);
            yield break;
        }

        QuizResponse data = JsonUtility.FromJson<QuizResponse>(request.downloadHandler.text);
        correctAnswer = data.correct;

        // Set answer texts
        for (int i = 0; i < choiceButtons.Length; i++)
        {
            TMP_Text btnText = choiceButtons[i].GetComponentInChildren<TMP_Text>();
            btnText.text = data.choices[i];

            int capturedIndex = i;
            choiceButtons[i].GetComponent<Button>().onClick.RemoveAllListeners();
            choiceButtons[i].GetComponent<Button>().onClick.AddListener(() => OnChoiceClicked(capturedIndex));
        }

        // Download and play audio
        StartCoroutine(DownloadAudio(data.audio_file));

        SetState(GameState.Select);
    }

    IEnumerator DownloadAudio(string filename)
    {
        string audioUrl = $"http://127.0.0.1:8000/audio/{filename}";
        UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(audioUrl, AudioType.WAV);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            audioSource.clip = DownloadHandlerAudioClip.GetContent(request);
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Failed to download audio: " + request.error);
        }
    }

    public void OnChoiceClicked(int index)
    {
        SetState(GameState.Decision);

        string chosenText = choiceButtons[index].GetComponentInChildren<TMP_Text>().text;

        if (chosenText == correctAnswer)
        {
            feedbackText.text = "Correct! Well done!";
        }
        else
        {
            feedbackText.text = "Incorrect! So sad...";
        }

        SetState(GameState.Result);
    }

    void RestartGame()
    {
        SetState(GameState.Start);
    }

    public void OnReplayAudio()
    {
        if (audioSource.clip != null)
            audioSource.Play();
    }
}
