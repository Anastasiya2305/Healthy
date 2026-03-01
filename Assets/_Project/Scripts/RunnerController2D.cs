using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class RunnerController2D : MonoBehaviour
{
    [SerializeField] private float jumpVelocity = 8f;
    [SerializeField] private float groundCheckDistance = 0.1f;
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float fallGravityScale = 3f;

    private Rigidbody2D rb;
    private Collider2D col;
    private float baseGravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        baseGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        bool jumpPressed = Keyboard.current?.spaceKey.wasPressedThisFrame == true
                           || Mouse.current?.leftButton.wasPressedThisFrame == true
                           || Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true;

        if (jumpPressed && IsGrounded())
        {
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpVelocity;
            rb.linearVelocity = velocity;
        }

        bool holdPressed = Keyboard.current?.spaceKey.isPressed == true
                           || Mouse.current?.leftButton.isPressed == true
                           || Touchscreen.current?.primaryTouch.press.isPressed == true;

        rb.gravityScale = holdPressed && rb.linearVelocity.y < 0f ? fallGravityScale : baseGravityScale;
    }

    private bool IsGrounded()
    {
        Bounds bounds = col.bounds;
        Vector2 origin = new Vector2(bounds.center.x, bounds.min.y + 0.01f);
        float width = Mathf.Max(0.05f, bounds.extents.x * 0.95f);
        RaycastHit2D hit = Physics2D.BoxCast(origin, new Vector2(width * 2f, 0.05f), 0f, Vector2.down, groundCheckDistance, groundMask);
        return hit.collider != null;
    }
}
