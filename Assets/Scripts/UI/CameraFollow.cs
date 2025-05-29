using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float followSpeed = 5f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null) return;

        // Desired position directly over the player (preserve camera's Z)
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, transform.position.z);

        // Smoothly move toward the target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 1f / followSpeed);
    }
}
