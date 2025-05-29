using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Map Reference")]
    public GameObject mapObject;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    private Vector2 movement;
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private Vector2 playerExtents;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (mapObject != null)
        {
            MapUtils.GetMapBounds(mapObject, out minBounds, out maxBounds);
        }
        else
        {
            Debug.LogWarning("Map object not assigned to PlayerMovement.");
        }

        // Convert player size from local to world space
        playerExtents = boxCollider.bounds.extents;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        Vector2 newPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        // Clamp using the player's physical size
        float clampedX = Mathf.Clamp(newPos.x, minBounds.x + playerExtents.x, maxBounds.x - playerExtents.x);
        float clampedY = Mathf.Clamp(newPos.y, minBounds.y + playerExtents.y, maxBounds.y - playerExtents.y);
        rb.MovePosition(new Vector2(clampedX, clampedY));

        if (movement.x != 0)
        {
            GetComponent<SpriteRenderer>().flipX = movement.x < 0;
        }
    }
}
