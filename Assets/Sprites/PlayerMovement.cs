using UnityEditor;
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

    private bool wasGroundedLastFrame = false;
    public bool EditorVisual = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = GetComponent<BoxCollider>();
    }

    private bool IsGrounded()
    {
        Vector3 boxCenter = transform.position + groundChecker.center + Vector3.down * (groundChecker.size.y / 2 - 0.35f);
        Vector3 boxSize = new Vector3(groundChecker.size.x * 1.1f, 0.8f, groundChecker.size.z * 0.9f);

        return Physics.CheckBox(boxCenter, boxSize / 2, Quaternion.identity, groundLayer);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>().x * playerSpeed;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, playerJumpForce, rb.linearVelocity.z);
                jumpCount = 1;
            }
            else if (jumpCount < maxJumpCount)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, playerJumpForce, rb.linearVelocity.z);
                jumpCount++;
            }
        }
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            print("Attack");
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(_movement, rb.linearVelocity.y, rb.linearVelocity.z);
    }

    private void Update()
    {
        bool grounded = IsGrounded();

        if (grounded && !wasGroundedLastFrame)
        {
            jumpCount = 0;
        }

        wasGroundedLastFrame = grounded;
    }

    void OnDrawGizmos()
    {
        if (EditorVisual)
        {
            if (groundChecker == null)
                groundChecker = GetComponent<BoxCollider>();

            Vector3 boxCenter = transform.position + groundChecker.center + Vector3.down * (groundChecker.size.y / 2 - 0.35f);
            Vector3 boxSize = new Vector3(groundChecker.size.x * 1.1f, 0.8f, groundChecker.size.z * 0.9f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(boxCenter, boxSize);
        }
    }
}
