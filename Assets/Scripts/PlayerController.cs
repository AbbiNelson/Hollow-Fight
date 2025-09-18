using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float playerSpeed = 5f;
    public float playerJumpForce = 5f;

    private Rigidbody rb;
    private BoxCollider groundChecker;

    private Vector2 _movement;
    public int jumpCount = 0;
    public int maxJumpCount = 2;

    public LayerMask groundLayer;
    public LayerMask wallLayer;

    private bool wasGroundedLastFrame = false;
    public bool EditorVisual = false;

    public ParticleSystem dustParticles;

    // cache emission for performance
    private ParticleSystem.EmissionModule dustEmission;

    public HealthSystem healthSystem;
    public Transform EnemyTest;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundChecker = GetComponent<BoxCollider>();

        if (dustParticles != null)
        {
            dustEmission = dustParticles.emission;
            dustEmission.rateOverTime = 0; // start disabled
        }
    }

    private bool IsGrounded(bool wall)
    {
        Vector3 boxCenter = transform.position + groundChecker.center + Vector3.down * (groundChecker.size.y / 2 - 0.35f);
        Vector3 boxSize = new Vector3(groundChecker.size.x * 1.1f, 0.8f, groundChecker.size.z * 0.9f);

        if (!WallCheck() && wall == true)
        {
            return Physics.CheckBox(boxCenter, boxSize / 2, Quaternion.identity, groundLayer | wallLayer);
        }
        return Physics.CheckBox(boxCenter, boxSize / 2, Quaternion.identity, groundLayer);
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>();
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            if (IsGrounded(true))
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, playerJumpForce, rb.linearVelocity.z);
                jumpCount = 1;

                // burst on jump
                CreateDustBurst();
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
            healthSystem.TakeDamage(10, EnemyTest);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(_movement.x * playerSpeed, rb.linearVelocity.y, rb.linearVelocity.z);

        if (_movement.x > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (_movement.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        bool grounded = IsGrounded(false);

        if (grounded || WallCheck() || !WallCheck())
        {
            // dynamically scale dust based on speed
            float speedFactor = Mathf.Abs(_movement.x * playerSpeed);
            dustEmission.rateOverTime = speedFactor * 2.5f; // tweak multiplier (5) for effect
        }
        else
        {
            // disable dust in air
            dustEmission.rateOverTime = 0;
        }
    }

    private Color lastGroundColor = Color.white;
    private RaycastHit lastHit;

    // Distance for raycasts
    public float checkDistance = 2f;
    public float sideOffset = 0.5f;

    private void UpdateDustColor()
    {
        RaycastHit hit;

        // Positions for center, left, and right checks
        Vector3 originCenter = transform.position;
        Vector3 originLeft = transform.position + Vector3.left * sideOffset;
        Vector3 originRight = transform.position + Vector3.right * sideOffset;

        // Check center first
        if (Physics.Raycast(originCenter, Vector3.left, out hit, checkDistance, groundLayer | wallLayer) ||
            Physics.Raycast(originLeft, Vector3.down, out hit, checkDistance, groundLayer | wallLayer) ||
            Physics.Raycast(originRight, Vector3.right, out hit, checkDistance, groundLayer | wallLayer))
        {
            lastHit = hit;

            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null && rend.material.HasProperty("_Color"))
            {
                lastGroundColor = rend.material.color;

                var main = dustParticles.main;
                main.startColor = new ParticleSystem.MinMaxGradient(lastGroundColor);
            }
        }
    }



    private bool WallCheck()
    {
        Vector3 boxCenter = transform.position + groundChecker.center + Vector3.down * (groundChecker.size.y / 2 - 0.35f);
        Vector3 boxSize = new Vector3(groundChecker.size.x * 1.1f, 0.8f, groundChecker.size.z * 0.9f);

        // Check for any colliders in the box
        Collider[] hits = Physics.OverlapBox(boxCenter, boxSize / 2, Quaternion.identity, wallLayer);

        foreach (Collider col in hits)
        {
            // Check if the GameObject has the WallType component
            WallType wall = col.GetComponent<WallType>();
            if (wall != null) // only return true if the bool is true
            {
                return wall.isSlippery;
            }
        }
        return false;
    }

    private void Update()
    {
        bool grounded = IsGrounded(false);

        if (grounded == false)
        {
            if (WallCheck())
            {
                //print("works1");
                jumpCount = maxJumpCount;

                //if (_movement.x > 0)
                //    transform.localScale = new Vector3(-1, 1, 1);
                //else if (_movement.x < 0)
                //    transform.localScale = new Vector3(1, 1, 1);

                return;
            }
            //else if (!WallCheck())
            //{
            //    print("works2");
            //    if (_movement.x > 0)
            //        transform.localScale = new Vector3(-1, 1, 1);
            //    else if (_movement.x < 0)
            //        transform.localScale = new Vector3(1, 1, 1);

            //    return;
            //}
        }

        if (grounded && !wasGroundedLastFrame)
        {
            jumpCount = 0;

            // landing burst
            CreateDustBurst();
        }


        wasGroundedLastFrame = grounded;
    }

    private void CreateDustBurst()
    {
        if (dustParticles != null)
        {
            UpdateDustColor(); // set color before emission
            dustParticles.Emit(10); // one-time burst of 10 particles
        }
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



            Gizmos.color = Color.yellow;

            // Draw rays from center, left, and right
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * checkDistance);
            Gizmos.DrawLine(transform.position + Vector3.left * sideOffset, transform.position + Vector3.left * sideOffset + Vector3.left * checkDistance);
            Gizmos.DrawLine(transform.position + Vector3.right * sideOffset, transform.position + Vector3.right * sideOffset + Vector3.right * checkDistance);

            // Show hit point sphere
            if (lastHit.collider != null)
            {
                Gizmos.color = lastGroundColor;
                Gizmos.DrawSphere(lastHit.point, 0.1f);
            }
        }
    }
}