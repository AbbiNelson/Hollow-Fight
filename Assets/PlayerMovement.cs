using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float playerJumpForce = 5f;

    private Rigidbody rb;

    private BoxCollider groundChecker;

    private float _movement;
    public int jumpCount = 0;
    public int maxJumpCount = 2;

    public LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = GetComponent<BoxCollider>();
        print(groundChecker.isTrigger);
    }

    private bool IsGrounded()
    {
        // Calculate the box center and size for the ground check
        Vector3 boxCenter = transform.position + groundChecker.center + Vector3.down * (groundChecker.size.y / 2 - 0.35f);
        Vector3 boxSize = new Vector3(groundChecker.size.x * 1.1f, 1.1f, groundChecker.size.z * 0.9f);

        return Physics.CheckBox(boxCenter, boxSize / 2, Quaternion.identity, groundLayer);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>().x * playerSpeed;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed && (IsGrounded() || jumpCount < maxJumpCount))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, playerJumpForce, rb.linearVelocity.z);
            jumpCount++;
        }
        else if (jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, playerJumpForce, rb.linearVelocity.z);
            jumpCount++;
        }
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        jumpCount = 0;
    //    }
    //}

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(_movement, rb.linearVelocity.y, rb.linearVelocity.z);

        if (IsGrounded())
        {
            jumpCount = 0;
        }
    }

    void OnDrawGizmos()
    {
        if (groundChecker == null)
            groundChecker = GetComponent<BoxCollider>();

        Vector3 boxCenter = transform.position + groundChecker.center + Vector3.down * (groundChecker.size.y / 2 - 0.35f);
        Vector3 boxSize = new Vector3(groundChecker.size.x * 1.2f, 1.1f, groundChecker.size.z * 0.9f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(boxCenter, boxSize);
    }
}