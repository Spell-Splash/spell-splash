using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class BackDoorButton : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 0.5f;

    public void OnBackButtonPressed()
    {
        StartCoroutine(ReturnToPreviousScene());
    }

    IEnumerator ReturnToPreviousScene()
    {
        // Fade out
        if (fadeCanvas) yield return StartCoroutine(Fade(1));

        string returnScene = SceneTransferManager.Instance.returnScene;
        Vector3 returnPos = SceneTransferManager.Instance.returnPosition;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(returnScene);
        while (!asyncLoad.isDone)
            yield return null;

        // Wait a frame
        yield return null;

        // Fade in
        if (fadeCanvas) yield return StartCoroutine(Fade(0));
    }

    IEnumerator Fade(float targetAlpha)
    {
        float t = 0f;
        float start = fadeCanvas.alpha;
        while (t < fadeDuration)
        {
            fadeCanvas.alpha = Mathf.Lerp(start, targetAlpha, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        fadeCanvas.alpha = targetAlpha;
    }
}
