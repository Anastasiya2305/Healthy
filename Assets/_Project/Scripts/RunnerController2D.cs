using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class RunnerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float runSpeed = 6f;

    [Header("Jump")]
    [SerializeField] private float jumpImpulse = 8f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float groundCheckDistance = 0.1f;

    [Header("Dive (Optional)")]
    [SerializeField] private bool enableDive = true;
    [SerializeField] private float diveGravityScale = 3f;

    private Rigidbody2D rb;
    private Collider2D bodyCollider;
    private float baseGravityScale;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<Collider2D>();
        baseGravityScale = rb.gravityScale;
    }

    private void Update()
    {
        if (IsJumpPressedThisFrame() && IsGrounded())
        {
            Jump();
        }

        if (enableDive)
        {
            bool shouldDive = IsJumpHeld() && !IsGrounded() && rb.velocity.y < 0f;
            rb.gravityScale = shouldDive ? diveGravityScale : baseGravityScale;
        }
        else
        {
            rb.gravityScale = baseGravityScale;
        }
    }

    private void FixedUpdate()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = runSpeed;
        rb.velocity = velocity;
    }

    private void Jump()
    {
        Vector2 velocity = rb.velocity;
        velocity.y = 0f;
        rb.velocity = velocity;
        rb.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
    }

    private bool IsGrounded()
    {
        Bounds bounds = bodyCollider.bounds;
        Vector2 castOrigin = new Vector2(bounds.center.x, bounds.min.y + 0.01f);
        Vector2 castSize = new Vector2(Mathf.Max(0.05f, bounds.size.x * 0.9f), 0.05f);
        RaycastHit2D hit = Physics2D.BoxCast(castOrigin, castSize, 0f, Vector2.down, groundCheckDistance, groundMask);
        return hit.collider != null;
    }

    private static bool IsJumpPressedThisFrame()
    {
        bool keyboardPressed = Keyboard.current?.spaceKey.wasPressedThisFrame == true;
        bool mousePressed = Mouse.current?.leftButton.wasPressedThisFrame == true;
        bool touchPressed = Touchscreen.current?.primaryTouch.press.wasPressedThisFrame == true;
        return keyboardPressed || mousePressed || touchPressed;
    }

    private static bool IsJumpHeld()
    {
        bool keyboardHeld = Keyboard.current?.spaceKey.isPressed == true;
        bool mouseHeld = Mouse.current?.leftButton.isPressed == true;
        bool touchHeld = Touchscreen.current?.primaryTouch.press.isPressed == true;
        return keyboardHeld || mouseHeld || touchHeld;
    }
}
