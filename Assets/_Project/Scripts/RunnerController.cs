using System;
using UnityEngine;

/// <summary>
/// Basic runner movement controller for MVP input.
/// Setup in scene:
/// 1) Add a Rigidbody + Collider (CapsuleCollider recommended) to the player object.
/// 2) Add this RunnerController component to the same object.
/// 3) Assign Ground Layer(s) in the inspector.
/// 4) Tune Jump Force, Extra Gravity Multiplier, and Ground Check Distance.
/// Controls:
/// - Tap Space / Left Mouse / Touch begin: Jump (when grounded).
/// - Hold input while airborne: Dive (faster descent through extra gravity).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class RunnerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private float lateralSpeed = 5f;

    [Header("Jump + Dive")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float extraGravityMultiplier = 2.5f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer = ~0;
    [SerializeField] private float groundCheckDistance = 0.12f;

    public event Action OnJump;
    public event Action<bool> OnLanded;

    private Rigidbody rb;
    private Collider bodyCollider;
    private bool wasGrounded;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        bodyCollider = GetComponent<Collider>();
    }

    private void Update()
    {
        if (!CanReadInput())
        {
            return;
        }

        bool grounded = IsGrounded();

        if (grounded && !wasGrounded)
        {
            // Hook: swap perfect-landing logic in later (timing/speed windows, lane alignment, etc.).
            bool perfectLanding = false;
            OnLanded?.Invoke(perfectLanding);
        }

        if (grounded && WantsJumpDown())
        {
            Jump();
            grounded = false;
        }

        wasGrounded = grounded;
    }

    private void FixedUpdate()
    {
        if (!CanSimulate())
        {
            return;
        }

        Vector3 velocity = rb.velocity;

        // Endless-run baseline motion.
        velocity.z = forwardSpeed;

        // Optional simple horizontal move for keyboard/controller.
        float horizontal = Input.GetAxisRaw("Horizontal");
        velocity.x = horizontal * lateralSpeed;

        // Hold-to-dive: increase descent while airborne.
        if (!IsGrounded() && IsHoldingDive() && velocity.y < 0f)
        {
            velocity += Physics.gravity * (extraGravityMultiplier * Time.fixedDeltaTime);
        }

        rb.velocity = velocity;
    }

    private void Jump()
    {
        Vector3 velocity = rb.velocity;
        velocity.y = 0f;
        rb.velocity = velocity;
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        OnJump?.Invoke();
    }

    private bool IsGrounded()
    {
        Bounds bounds = bodyCollider.bounds;
        Vector3 origin = bounds.center;
        float sphereRadius = Mathf.Max(0.05f, Mathf.Min(bounds.extents.x, bounds.extents.z) * 0.95f);
        float castDistance = bounds.extents.y + groundCheckDistance;

        return Physics.SphereCast(
            origin,
            sphereRadius,
            Vector3.down,
            out _,
            castDistance,
            groundLayer,
            QueryTriggerInteraction.Ignore);
    }

    private bool WantsJumpDown()
    {
        return Input.GetKeyDown(KeyCode.Space)
               || Input.GetMouseButtonDown(0)
               || Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    private bool IsHoldingDive()
    {
        bool touchHold = Input.touchCount > 0;
        return Input.GetKey(KeyCode.Space)
               || Input.GetMouseButton(0)
               || touchHold;
    }

    private bool CanReadInput()
    {
        GameManager gm = GameManager.Instance;
        return gm == null || gm.IsGameplayActive();
    }

    private bool CanSimulate()
    {
        GameManager gm = GameManager.Instance;
        return gm == null || gm.IsGameplayActive();
    }
}
