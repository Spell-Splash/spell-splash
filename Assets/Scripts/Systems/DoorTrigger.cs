using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DoorTrigger : MonoBehaviour
{
    [Header("Scene Settings")]
    public string targetSceneName;
    public string spawnPointTag = "SpawnPoint"; // Tag used in target scene

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (SceneTransferManager.Instance.IsReturningFromGuild)
        {
            SceneTransferManager.Instance.ClearReturnFlag();
            return;
        }

        SceneTransferManager.Instance.SaveReturnPoint(
            other.transform.position,
            SceneManager.GetActiveScene().name,
            Camera.main.transform.position
        );

        StartCoroutine(TransitionToScene(other.gameObject));
    }

    IEnumerator TransitionToScene(GameObject player)
    {
        player.GetComponent<PlayerMovement>()?.SetMovementEnabled(false);
        Debug.Log($"Transitioning to scene: {targetSceneName}");

        // Fade out
        if (fadeCanvas) yield return StartCoroutine(Fade(1));

        // Load scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetSceneName);
        while (!asyncLoad.isDone)
            yield return null;

        yield return null;

        // Move player to spawn point
        GameObject spawn = GameObject.FindWithTag(spawnPointTag);
        if (spawn)
            player.transform.position = spawn.transform.position;

        // Handle camera restoration
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.SetFollowEnabled(false); // Disable temporarily
            camFollow.ForcePosition(SceneTransferManager.Instance.returnCameraPosition);
        }

        // Delay enabling follow to next frame
        yield return null;

        if (camFollow != null)
            camFollow.SetFollowEnabled(true);

        // Fade in
        if (fadeCanvas) yield return StartCoroutine(Fade(0));

        SceneTransferManager.Instance.ClearReturnFlag();
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
