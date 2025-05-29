using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;

    private Vector3 velocity = Vector3.zero;

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    void Start()
    {
        Camera cam = Camera.main;
        cameraHalfHeight = cam.orthographicSize;
        cameraHalfWidth = cam.aspect * cameraHalfHeight;

        // TODO: Move this to GameManager or similar
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        GameObject map = GameObject.Find("Quaint Village in Pixel Art");
        camFollow.SetCameraBoundsFromMap(map);
    }

    void LateUpdate()
    {
        if (player == null) return;

        // Desired camera position (centered on player)
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);

        // Clamp only the camera position if its edge would leave the map
        float clampedX = ClampCameraAxis(targetPos.x, minBounds.x, maxBounds.x, cameraHalfWidth);
        float clampedY = ClampCameraAxis(targetPos.y, minBounds.y, maxBounds.y, cameraHalfHeight);

        Vector3 clampedPos = new Vector3(clampedX, clampedY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, 1f / followSpeed);
    }

    float ClampCameraAxis(float camCenter, float mapMin, float mapMax, float camHalf)
    {
        float min = mapMin + camHalf;
        float max = mapMax - camHalf;

        // If map is smaller than camera view, stay centered
        if (min > max) return (min + max) / 2;

        return Mathf.Clamp(camCenter, min, max);
    }

    public void SetCameraBoundsFromMap(GameObject mapObject)
    {
        SpriteRenderer renderer = mapObject.GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogError("Map object must have a SpriteRenderer.");
            return;
        }

        Bounds mapBounds = renderer.bounds;
        minBounds = mapBounds.min;
        maxBounds = mapBounds.max;
    }

    public Vector2 GetMinBounds() => minBounds;
    public Vector2 GetMaxBounds() => maxBounds;

}
