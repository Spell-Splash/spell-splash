using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneDoor : MonoBehaviour
{
    [Header("Scene Settings")]
    public string targetSceneName;
    public string spawnPointTag = "SpawnPoint"; // Tag used in target scene

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // TODO: Add guild map here if needed.
            // Optionally keep the player object across scenes
            // DontDestroyOnLoad(other.gameObject); // Keep player across scenes
            StartCoroutine(TransitionToScene(other.gameObject));
        }
    }

    IEnumerator TransitionToScene(GameObject player)
    {
        // Fade out
        if (fadeCanvas) yield return StartCoroutine(Fade(1));

        // Load scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        while (!asyncLoad.isDone)
            yield return null;

        // Wait a frame to allow scene to initialize
        yield return null;

        // Find spawn point in new scene
        GameObject spawn = GameObject.FindWithTag(spawnPointTag);
        if (spawn)
            player.transform.position = spawn.transform.position;

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
