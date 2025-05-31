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
    private SpriteRenderer spriteRenderer;
    private Vector2 movement;

    private Vector2 minBounds;
    private Vector2 maxBounds;
    private Vector2 playerExtents;

    private bool boundsInitialized = false;
    public static PlayerMovement Instance;
    private bool canMove = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (SceneTransferManager.Instance != null && SceneTransferManager.Instance.IsReturningFromGuild)
        {
            transform.position = SceneTransferManager.Instance.returnPosition;
        }

        InitializeBounds();
    }

    void InitializeBounds()
    {
        if (mapObject != null)
        {
            MapUtils.GetMapBounds(mapObject, out minBounds, out maxBounds);

            // Important: refresh bounds after scene is loaded and object is positioned
            playerExtents = boxCollider.bounds.extents;
            boundsInitialized = true;
        }
        else
        {
            Debug.LogWarning("[PlayerMovement] Map object not assigned.");
        }
    }

    void Update()
    {
        if (!canMove)
        {
            movement = Vector2.zero;
            return;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        if (!canMove) return;

        Vector2 newPos = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        if (boundsInitialized)
        {
            float clampedX = Mathf.Clamp(newPos.x, minBounds.x + playerExtents.x, maxBounds.x - playerExtents.x);
            float clampedY = Mathf.Clamp(newPos.y, minBounds.y + playerExtents.y, maxBounds.y - playerExtents.y);
            newPos = new Vector2(clampedX, clampedY);
        }

        rb.MovePosition(newPos);

        if (movement.x != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
    }

    public bool IsMovementEnabled()
    {
        return canMove;
    }
}
