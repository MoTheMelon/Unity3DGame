using UnityEngine;

/*
    This script provides jumping and movement in Unity 3D - Gatsby
*/


public class Player : MonoBehaviour
{
    // Camera Rotation
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    private Transform cameraTransform;

    // Ground Movement
    private Rigidbody rb;
    public float MoveSpeed = 5f;
    private float moveHorizontal;
    private float moveForward;

    // Jumping
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f; // Multiplies gravity when falling down
    public float ascendMultiplier = 2f; // Multiplies gravity for ascending to peak of jump
    private bool isGrounded = true;
    public LayerMask groundLayer;
    private float groundCheckTimer = 0f;
    private float groundCheckDelay = 0.3f;
    private float playerHeight;
    private float raycastDistance;

    //selecting objects
    private GameObject currentHint;
    private GameObject currentTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        cameraTransform = Camera.main.transform;

        // Set the raycast to be slightly beneath the player's feet
        playerHeight = GetComponent<CapsuleCollider>().height * transform.localScale.y;
        raycastDistance = (playerHeight / 2) + 0.2f;

        // Hides the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveForward = Input.GetAxisRaw("Vertical");

        RotateCamera();

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        ApplyPickUpHint();

        // Checking when we're on the ground and keeping track of our ground check delay

        if (groundCheckTimer <= 0f)
        {
            Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
            Ray ray = new Ray(rayOrigin, Vector3.down);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, groundLayer))
            {
                // Only grounded if the surface normal is mostly upward
                isGrounded = Vector3.Angle(hit.normal, Vector3.up) < 45f;
            }
            else
            {
                isGrounded = false;
            }
        }
        else
        {
            groundCheckTimer -= Time.deltaTime;
        }


    }

    void FixedUpdate()
    {
        MovePlayer();
        ApplyJumpPhysics();
    }

    void MovePlayer()
    {

        Vector3 movement = (transform.right * moveHorizontal + transform.forward * moveForward).normalized;
        Vector3 targetVelocity = movement * MoveSpeed;

        // Apply movement to the Rigidbody
        Vector3 velocity = rb.velocity;
        velocity.x = targetVelocity.x;
        velocity.z = targetVelocity.z;
        rb.velocity = velocity;

        // If we aren't moving and are on the ground, stop velocity so we don't slide
        if (isGrounded && moveHorizontal == 0 && moveForward == 0)
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
    }

    void RotateCamera()
    {

        //rotates the whole body and camera attached to body rotates with it
        float horizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, horizontalRotation, 0); //transform is the object body location, rotation, and scale

        //angles camera up and down with a clamp so you cant look 180 back at yourself
        verticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    void Jump()
    {
        isGrounded = false;
        groundCheckTimer = groundCheckDelay;
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z); // Initial burst for the jump
    }

    void ApplyJumpPhysics()
    {
        if (rb.velocity.y < 0)
        {
            // Falling: Apply fall multiplier to make descent faster
            rb.velocity += Vector3.up * Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime;
        } // Rising
        else if (rb.velocity.y > 0)
        {
            // Rising: Change multiplier to make player reach peak of jump faster
            rb.velocity += Vector3.up * Physics.gravity.y * ascendMultiplier * Time.fixedDeltaTime;
        }
    }

    void ApplyPickUpHint()
    {
        // Draw debug ray from the center of the camera's view forward
        Vector3 origin = cameraTransform.position;
        Vector3 direction = cameraTransform.forward;

        float rayLength = 3f;
        Ray pickUpRay = new Ray(origin, direction * rayLength);
        Debug.DrawRay(origin, direction * rayLength, Color.red);

        if (Physics.Raycast(pickUpRay, out RaycastHit pick, rayLength))
        {
            GameObject target = pick.collider.gameObject;

            

            if (target.CompareTag("PickUp"))
            {
                //Debug.Log("Looking at a pickup object: " + target.name);
                Transform child = target.transform.Find("outline");

                if (currentTarget == null)
                {
                    currentTarget = target;
                }

                if (currentTarget != target)
                {
                    Debug.Log("current " + currentTarget.name + " target " + target.name);
                    //new target is diff from old without looking away
                    if (currentTarget.transform.Find("outline") != null)
                    {
                        currentTarget.transform.Find("outline").gameObject.SetActive(false);
                    }
                    currentTarget = target;

                }
                else
                {
                    //should be same as what we're looking at now
                    currentTarget = target;
                }

                if (child != null)
                {
                    currentHint = child.gameObject;
                    currentHint.SetActive(true);
                }

                if (Input.GetMouseButtonDown(0)) // LMB click
                {
                    Debug.Log("Picked up: " + target.name);
                    // TODO: Call your pickup logic here
                }
            }
        }
        else
        {
            //ray isnt hitting anything, so turn off last hint
            if (currentHint != null)
            {
                currentHint.SetActive(false);
            }
            currentTarget = null;

        }
    }



}