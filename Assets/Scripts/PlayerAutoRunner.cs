using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerAutoRunner : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 6f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private LayerMask groundMask = ~0;
    [SerializeField] private float groundCheckDistance = 0.1f;

    private Rigidbody rb;
    private CapsuleCollider capsuleCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        var velocity = rb.velocity;
        velocity.z = forwardSpeed;
        rb.velocity = velocity;
    }

    private void Update()
    {
        if (!WantsJump())
        {
            return;
        }

        if (IsGrounded())
        {
            var velocity = rb.velocity;
            velocity.y = 0f;
            rb.velocity = velocity;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool WantsJump()
    {
        return Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
    }

    private bool IsGrounded()
    {
        Vector3 center = transform.TransformPoint(capsuleCollider.center);
        float radius = Mathf.Max(0.01f, capsuleCollider.radius * 0.95f);
        float castDistance = (capsuleCollider.height * 0.5f) - capsuleCollider.radius + groundCheckDistance;
        return Physics.SphereCast(center, radius, Vector3.down, out _, castDistance, groundMask, QueryTriggerInteraction.Ignore);
    }
}
