using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector2 viewSize = new Vector2(16f, 9f); // size of visible area (camera width x height)
    public float transitionSpeed = 10f;

    private Vector3 targetPosition;

    void Start()
    {
        if (player == null) return;
        // Snap camera to initial chunk
        targetPosition = GetChunkPosition(player.position);
        transform.position = targetPosition;
    }

    void Update()
    {
        if (player == null) return;

        Vector3 playerPos = player.position;
        Vector3 camPos = transform.position;

        // If player walks outside of the current camera bounds, move to new chunk
        if (Mathf.Abs(playerPos.x - camPos.x) > viewSize.x / 2f ||
            Mathf.Abs(playerPos.y - camPos.y) > viewSize.y / 2f)
        {
            targetPosition = GetChunkPosition(playerPos);
        }

        // Smooth camera movement
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, transitionSpeed * Time.deltaTime);
    }

    // Snap target camera position to center of next chunk
    Vector3 GetChunkPosition(Vector3 pos)
    {
        float chunkX = Mathf.Floor(pos.x / viewSize.x) * viewSize.x + viewSize.x / 2f;
        float chunkY = Mathf.Floor(pos.y / viewSize.y) * viewSize.y + viewSize.y / 2f;
        return new Vector3(chunkX, chunkY, transform.position.z);
    }
}
