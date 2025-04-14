using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MovePlayer : MonoBehaviour
{
    private CharacterController controller; // Reference to the CharacterController component

    [SerializeField] Transform portal;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float defaultSpeed = 12f; // Normal walking speed
    [SerializeField] private float runSpeed = 12f; // Running speed

    [SerializeField] private float gravity = -9.81f; // Gravity force
    [SerializeField] private float jumpHeight = 3f; // Height of the jump

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 5f; // Maximum stamina value
    private float stamina; // Current stamina
    private bool canRun = true; // If the player is allowed to run

    [Header("UI Settings")]
    public Slider staminaBar; // UI slider to display stamina

    private Vector3 velocity; // Stores player's vertical movement (gravity/jump)
    private bool isRunning; // Checks if the player is running
    private bool wasGroundedLastFrame = true; // Tracks landing event

    [Header("Footstep Settings")]
    [SerializeField] private float walkStepRate = 0.5f; // Time between footsteps when walking
    [SerializeField] private float runStepRate = 0.3f; // Time between footsteps when running
    private float nextStepTime = 0f; // Time tracker for next footstep sound

    private Footsteps Footsteps; // Reference to the Footsteps script

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Footsteps = FindFirstObjectByType<Footsteps>(); // Find the Footsteps component
        playerSpeed = defaultSpeed;
        stamina = maxStamina;

        // Initialize stamina UI
        if (staminaBar != null)
        {
            staminaBar.maxValue = maxStamina;
            staminaBar.value = stamina;
        }
    }

    void Update()
    {
        HandleMovement(); // Handles player movement
        HandleGravity();  // Applies gravity to the player
        HandleJump();     // Handles jumping input
        HandleRun();      // Handles running and stamina management


        //cheat
        if (Input.GetKeyDown(KeyCode.T))
        {
            this.transform.position = portal.transform.position;    
        }
    }

    private void HandleRun()
    {
        // Running logic: Hold Shift to run if stamina is available
        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && canRun)
        {
            playerSpeed = runSpeed;
            stamina -= Time.deltaTime;

            if (stamina <= 0)
                canRun = false; // Prevent running when stamina is depleted

            isRunning = true;
        }
        else
        {
            playerSpeed = defaultSpeed;

            // Regenerate stamina when not running
            if (stamina < maxStamina)
                stamina += Time.deltaTime * 0.5f;

            if (stamina >= maxStamina)
                canRun = true; // Allow running again when stamina is full

            isRunning = false;
        }

        // Update stamina bar UI
        if (staminaBar != null)
        {
            staminaBar.value = stamina;
        }
    }

    private void HandleJump()
    {
        // Jump when the player is grounded
        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            Footsteps.PlayJumpAudio(); // Play jump sound
        }
    }

    private void HandleGravity()
    {
        // Apply gravity effect
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Reset vertical velocity when grounded
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;

            // Detect landing and play sound
            if (!wasGroundedLastFrame)
            {
                Footsteps.PlayLandAudio();
            }
        }

        wasGroundedLastFrame = controller.isGrounded; // Track ground status
    }

    private void HandleMovement()
    {
        // Get movement input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Move in the direction of input
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * playerSpeed * Time.deltaTime);

        // Play footstep sounds when moving
        if (move.magnitude > 0 && Time.time >= nextStepTime && controller.isGrounded)
        {
            Footsteps.PlayFootStepAudio();
            nextStepTime = Time.time + (isRunning ? runStepRate : walkStepRate);
        }
    }


  
    
}
