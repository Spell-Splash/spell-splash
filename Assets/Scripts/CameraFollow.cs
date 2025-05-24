using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;       // The player to follow
    public float smoothSpeed = 5f; // How smooth the camera follows
    public Vector3 offset;         // Optional offset

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}
