//Main author: William Örnquist
//Secondary author: Mattias Larsson
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform groundCheck; //Alternative groundCheck
    [SerializeField] private Transform crouchHeadTransform; //The position of the head in a crouched state
    [SerializeField] private GameObject playerHead; //The parent object to the camera holder object
    private Vector3 playerHeadPositionOrigin; //The camholder's original position (non-crouch state)

    private CamAnimation camAnimation;
    private PlayerVariables playerVariables;
    private AudioManager audioManager;
    private UiManager uiManager;
    private CharacterController controller;

    [Header("Misc")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask headMask;
    [SerializeField] private float groundCheckRadius = 0.4f; //The radius of the CheckSphere for 'groundCheck'

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float sprintCooldown;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchingHeight;

    [Header("Stamina")]
    [SerializeField] private float staminaUsedForSprint;
    [SerializeField] private float staminaUsedForDodge;
    [SerializeField] private float staminaUsedTimeSlow;
    [SerializeField] private float staminaUsedForJump;
    [SerializeField] private float sprintExhaustionTime;

    [Header("Slow motion")]
    [SerializeField] private float slowMotionAmountMultiplier;
    [SerializeField] private float slowMotionStaminaToBeUsedPerTick;
    [SerializeField] private float slowMotionTickTime;
    [SerializeField] private float slowMotionExhaustionTime;
    [SerializeField] private float slowMotionMovementSpeed;

    [Header("Dodge")]
    [SerializeField] private float dodgeAmountOfTime;
    [SerializeField] private float dodgeSpeedMultiplier;
    [SerializeField] private float maxDoubleTapTime = 0.3f;
    [SerializeField] private float dodgeCooldown;

    private Vector3 dodgeDirection;
    private Vector3 yVelocity; //This vector is used for storing added gravity every frame, building up downward velocity

    //Input and movement
    public static bool IsMoving { get; private set; }
    public static bool IsSprinting { get; private set; }
    public static bool IsDodging { get; private set; }
    public static bool IsCrouching { get; private set; }
    public static bool IsGrounded { get; private set; }

    private bool isSlowmotion;
    private bool breakSlowMotion;
    private bool isExhaustedFromSlowMotion;
    private bool isExhaustedFromSprinting;

    private float standingHeight;
    private float currentSpeed;
    private float inputX;
    private float inputZ;

    //Dodge timers
    private float rightDoubleTapTimer, leftDoubleTapTimer, dodgeCooldownTimer, dodgeTimer;

    //Other timers
    private float slowMotionExhaustionTimer, slowMotionTickTimer, sprintExhaustionTimer;

    //String cache
    private readonly string horizontal = "Horizontal";
    private readonly string vertical = "Vertical";

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        camAnimation = GetComponentInChildren<CamAnimation>();
        playerVariables = GetComponent<PlayerVariables>();
        audioManager = FindObjectOfType<AudioManager>();
        uiManager = FindObjectOfType<UiManager>();

        standingHeight = controller.height;
        playerHeadPositionOrigin = playerHead.transform.localPosition;
        Debug.Log(playerHeadPositionOrigin);
       
        //Prevents player from dodging at start
        rightDoubleTapTimer = maxDoubleTapTime;
        leftDoubleTapTimer = maxDoubleTapTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isSlowmotion && Input.GetKeyDown(KeyCode.Q) || isSlowmotion && playerVariables.GetCurrentStamina() <= 0f || !PlayerVariables.isAlive)
            StopTimeSlow();

        if (PlayerVariables.isAlive) //Disables movement when player is dead
        {
            //Checks if player is grounded and resets gravity velocity if true
            CheckGround();

            //Input checks
            CheckMovement();
            CheckCrouch();
            CheckSprint();
            CheckDodge();
            CheckTimeSlow();
        }
            //Applies gravity and jump velocity
        ApplyYAxisVelocity();
    }


    public float GetHorizontalInput() { return inputX; }
    public float GetVerticalInput() { return inputZ; }
    public Vector3 GetDodgeDirection() { return dodgeDirection; }

    private void CheckMovement()
    {
        //WASD Input
        inputX = Input.GetAxis(horizontal);
        inputZ = Input.GetAxis(vertical);
        Vector3 move = transform.right * inputX + transform.forward * inputZ;

        if (controller.isGrounded && inputX != 0 || controller.isGrounded && inputZ != 0)
            IsMoving = true;
        else
            IsMoving = false;
        //Applying WASD- or Dodge-movement based on 'Dodge'-state
        if (!IsDodging)
            controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void CheckSprint()
    {
        IsSprinting = (!IsDodging && !IsCrouching && Input.GetKey(KeyCode.LeftShift) && 
            playerVariables.GetCurrentStamina() > 0 && inputZ == 1 && inputX == 0 && !isExhaustedFromSprinting);

        if (IsSprinting)
        {
            currentSpeed = sprintSpeed;
            playerVariables.StaminaToBeUsed(staminaUsedForSprint * Time.deltaTime);

            if (playerVariables.GetCurrentStamina() < 1f)
            {
                isExhaustedFromSprinting = true;
                uiManager.SprintExhaustion(isExhaustedFromSprinting);
                sprintExhaustionTimer = sprintExhaustionTime;
            }
        }
        else if (!IsCrouching)
            currentSpeed = walkSpeed;


        if (isExhaustedFromSprinting && sprintExhaustionTimer <= 0f)
        {
            isExhaustedFromSprinting = false;
            uiManager.SprintExhaustion(isExhaustedFromSprinting);
        }
        else if (isExhaustedFromSprinting && sprintExhaustionTimer > 0f)
            sprintExhaustionTimer -= Time.unscaledDeltaTime;
    }

    private void CheckDodge()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (rightDoubleTapTimer > 0)
            {
                Debug.Log("Double tap right!");
                Dodge(transform.right);
                camAnimation.PlayDodgeRight();
            }
            rightDoubleTapTimer = maxDoubleTapTime;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (leftDoubleTapTimer > 0)
            {
                Debug.Log("Double tap left!");
                Dodge(-transform.right);
                camAnimation.PlayDodgeLeft();
            }
            leftDoubleTapTimer = maxDoubleTapTime;
        }

        if (IsDodging && dodgeTimer > 0f)
            ApplyDodge(); //Applies the horizontal force for the duration.
        else
            IsDodging = false;

        //Timers
        if (dodgeCooldownTimer > 0)
        {
            dodgeCooldownTimer -= Time.deltaTime;
            uiManager.DodgeCooldown(dodgeCooldownTimer, dodgeCooldown);
        }

        if (rightDoubleTapTimer > 0)
            rightDoubleTapTimer -= Time.deltaTime;
        if (leftDoubleTapTimer > 0)
            leftDoubleTapTimer -= Time.deltaTime;
    }

    private void Dodge(Vector3 direction) //This method is called once for the activation of dodge, resetting appropriate timers.
    {
        if (!IsDodging && IsGrounded && playerVariables.GetCurrentStamina() > staminaUsedForDodge && dodgeCooldownTimer <= 0)
        {
            IsDodging = true;
            audioManager.Play("PlayerDodgeSound");
            dodgeTimer = dodgeAmountOfTime;
            playerVariables.StaminaToBeUsed(staminaUsedForDodge);
            dodgeDirection = direction * currentSpeed * dodgeSpeedMultiplier;
            dodgeCooldownTimer = dodgeCooldown;
        }
    }

    private void ApplyDodge() //While dodge is active, this method is called every frame to apply force for the dodge mechanic.
    {
        if (Time.timeScale < 1f)
            controller.Move(dodgeDirection * currentSpeed * dodgeSpeedMultiplier * slowMotionAmountMultiplier * Time.unscaledDeltaTime);
        else
            controller.Move(dodgeDirection * currentSpeed * dodgeSpeedMultiplier * Time.deltaTime);

        dodgeTimer -= Time.deltaTime;
    }

    private void CheckTimeSlow()
    {

        if (Input.GetKeyDown(KeyCode.Q) && playerVariables.GetCurrentStamina() > staminaUsedTimeSlow && !isExhaustedFromSlowMotion)
        {
            playerVariables.StaminaToBeUsed(staminaUsedTimeSlow);
            isSlowmotion = true;
            Time.timeScale = slowMotionAmountMultiplier;
            currentSpeed = slowMotionMovementSpeed;
            Debug.Log("Slow motion active, time scale : " + Time.timeScale);
            audioManager.Play("SlowMoStart");
            uiManager.SlowMotionEffect();

            //Reset timers
            slowMotionTickTimer = slowMotionTickTime;
            slowMotionExhaustionTimer = slowMotionExhaustionTime;
        }

        if (isExhaustedFromSlowMotion && slowMotionExhaustionTimer <= 0f)
        {
            isExhaustedFromSlowMotion = false;
            uiManager.SlowMotionExhaustion(isExhaustedFromSlowMotion);
        }
        else if (isExhaustedFromSlowMotion && slowMotionExhaustionTimer > 0f)
            slowMotionExhaustionTimer -= Time.deltaTime;

        if (isSlowmotion)
            UpdateSlowMotion();
    }

    private void StopTimeSlow()
    {
        Debug.Log("Slow motion stops");
        isSlowmotion = false;

        if (PlayerVariables.isAlive)
        {
            isExhaustedFromSlowMotion = true;
            uiManager.SlowMotionExhaustion(isExhaustedFromSlowMotion);
        }

        RestoreTime();
    }

    private void CheckCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            //The player cannot stand up after crouching if the CheckSphere hovering above the player collides with an object. 
            if (IsCrouching && !Physics.CheckSphere(playerHead.transform.position + Vector3.up * crouchingHeight, 0.6f, headMask))
            {
                IsCrouching = false;
                currentSpeed = walkSpeed;
                controller.enabled = false; //CharacterController module MUST be disabled before directly changing the player's transform position.
                controller.height = standingHeight;
                controller.center -= Vector3.up * (standingHeight - crouchingHeight) / 2;
                transform.position += new Vector3(0, (standingHeight - crouchingHeight) / 2);
                controller.enabled = true;
            }
            else if (!IsCrouching)
            {
                IsCrouching = true;
                currentSpeed = crouchSpeed;
                controller.enabled = false;
                controller.height = crouchingHeight;
                controller.center += Vector3.up * (standingHeight - crouchingHeight) / 2;
                transform.position -= new Vector3(0, (standingHeight - crouchingHeight));
                controller.enabled = true;
            }

            uiManager.CrouchingImage(IsCrouching);
        }
    }

    public void TeleportPlayer(Transform location) //This method teleports the player during respawn. (from death)
    {
        currentSpeed = 0;
        controller.enabled = false;
        Debug.Log("Teleport activated on position " + location);
        transform.position = location.position;
        transform.rotation = location.rotation;
        controller.enabled = true;
    }

    public void TeleportPlayer(Vector3 location, Quaternion rotation) //This method teleports the player during respawn. (from loading)
    {
        if(controller == null)
        {
            Debug.Log("Controller was null on teleport");
            controller = GetComponent<CharacterController>();
        }
        controller.enabled = false;
        Debug.Log("Teleport activated on position " + location);
        transform.position = location;
        transform.rotation = rotation;
        controller.enabled = true;
    }

    private void CheckGround()
    {
        //The alternative check if player is grounded by casting CheckSphere.
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        //Resets gravity force when the character is grounded. This prevents gravity buildup.
        if (controller.isGrounded && yVelocity.y < 0)
            yVelocity.y = -2f;
    }

    private void ApplyYAxisVelocity()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && IsGrounded && !IsDodging && playerVariables.GetCurrentStamina() > staminaUsedForJump && PlayerVariables.isAlive)
        {
            playerVariables.StaminaToBeUsed(staminaUsedForJump);
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            audioManager.Play("playerJump");
        }

        //This builds up the downward velocity vector with gravity over time.
        yVelocity.y += gravity * Time.deltaTime;

        //This updates the player's position with the downward velocity. This is multiplied by deltaTime again for the formula of gravitation.
        controller.Move(yVelocity * Time.deltaTime);
    }

    private void UpdateSlowMotion()
    {
        if (slowMotionTickTimer > 0f)
            slowMotionTickTimer -= Time.unscaledDeltaTime;
        else if (slowMotionTickTimer <= 0f)
        {
            slowMotionTickTimer = slowMotionTickTime;
            playerVariables.StaminaToBeUsed(slowMotionStaminaToBeUsedPerTick);
        }
    }

    private void RestoreTime()
    {
        currentSpeed = walkSpeed;
        Time.timeScale = 1f; //returns to normal time
        isSlowmotion = false;
        breakSlowMotion = false;
        uiManager.SlowMotionEffect();
        Debug.Log("Time has restored to : " + Time.timeScale);
        audioManager.Play("SlowMoFinish");
    }

    private void OnDrawGizmos()
    {
        if (IsCrouching)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(playerHead.transform.position + Vector3.up * crouchingHeight, 0.6f);
        }
    }
}
