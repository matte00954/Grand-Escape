//Main author: William Örnquist
//Secondary author: Mattias Larsson
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [Header("References")]
    [SerializeField] private Transform groundCheck; //The groundCheck object.
    [SerializeField] private GameObject camHolder;
    private Vector3 camHolderPositionOrigin;

    private CamAnimation camAnimation;
    private PlayerVariables playerVariables;
    private AudioManager audioManager;
    private UiManager uiManager;
    private CharacterController controller;

    [Header("Misc")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask headMask;
    [SerializeField] private float groundCheckRadius = 0.4f; //The radius of the CheckSphere for 'groundCheck'.

    [Header("Movement")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float crouchHeight;
    [SerializeField] private float sprintCooldown;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float crouchSpeed;

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
    private bool isCrouching;
    private bool isSlowmotion;
    private bool isGrounded;
    private bool breakSlowMotion;
    private bool isExhaustedFromSlowMotion;
    private bool isExhaustedFromSprinting;

    private float currentSpeed;
    private float standingHeight;
    private float inputX;
    private float inputZ;

    //Dodge timers
    private float rightDoubleTapTimer, leftDoubleTapTimer, dodgeCooldownTimer, dodgeTimer;

    //Other timers
    private float slowMotionExhaustionTimer, slowMotionTickTimer, sprintExhaustionTimer;

    private readonly string horizontal = "Horizontal";
    private readonly string vertical = "Vertical";

    private void Start()
    {
        camAnimation = GetComponentInChildren<CamAnimation>();
        playerVariables = GetComponent<PlayerVariables>();
        controller = GetComponent<CharacterController>();
        audioManager = FindObjectOfType<AudioManager>();
        uiManager = FindObjectOfType<UiManager>();
        standingHeight = controller.height;
        camHolderPositionOrigin = camHolder.transform.position;
       
        //To prevent player from dodging at start
        rightDoubleTapTimer = maxDoubleTapTime;
        leftDoubleTapTimer = maxDoubleTapTime;
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerVariables.isAlive)
        {
            //Checks if player is grounded and resets gravity velocity if true
            CheckGround();

            //Input checks
            CheckMovement();
            CheckCrouch();
            CheckSprint();
            CheckDodge();
            CheckTimeSlow();

            //Applies gravity and jump velocity
            ApplyYAxisVelocity();
        }
    }

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
        IsSprinting = (!IsDodging && !isCrouching && Input.GetKey(KeyCode.LeftShift) && 
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
        else
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
            ApplyDodge();
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

    private void Dodge(Vector3 direction)
    {
        if (!IsDodging && isGrounded && playerVariables.GetCurrentStamina() > staminaUsedForDodge && dodgeCooldownTimer <= 0)
        {
            IsDodging = true;
            dodgeTimer = dodgeAmountOfTime;
            playerVariables.StaminaToBeUsed(staminaUsedForDodge);
            dodgeDirection = direction * currentSpeed * dodgeSpeedMultiplier;
            dodgeCooldownTimer = dodgeCooldown;
        }
    }

    private void ApplyDodge()
    {
        if (Time.timeScale < 1f)
            controller.Move((dodgeDirection * currentSpeed * dodgeSpeedMultiplier) * slowMotionAmountMultiplier * Time.unscaledDeltaTime);
        else
            controller.Move(dodgeDirection * currentSpeed * dodgeSpeedMultiplier * Time.deltaTime);

        dodgeTimer -= Time.deltaTime;
    }

    private void CheckTimeSlow()
    {
        if (isSlowmotion && Input.GetKeyDown(KeyCode.Q) || isSlowmotion && playerVariables.GetCurrentStamina() <= 0f)
        {
            Debug.Log("Slow motion stops");
            isSlowmotion = false;
            isExhaustedFromSlowMotion = true;
            uiManager.SlowMotionExhaustion(isExhaustedFromSlowMotion);
            RestoreTime();
        }

        if (Input.GetKeyDown(KeyCode.Q) && playerVariables.GetCurrentStamina() > staminaUsedTimeSlow && !isExhaustedFromSlowMotion && !isSlowmotion)
        {
            playerVariables.StaminaToBeUsed(staminaUsedTimeSlow);
            isSlowmotion = true;
            Time.timeScale = slowMotionAmountMultiplier;
            currentSpeed = slowMotionMovementSpeed;
            Debug.Log("Slow motion active, time scale : " + Time.timeScale);
            audioManager.Play("SlowMoStart");

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

    private void CheckCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isCrouching && Physics.CheckSphere(camHolderPositionOrigin, 0.6f, headMask))
            {
                isCrouching = false;
                currentSpeed = walkSpeed;
            }

            //if (isCrouching)
            //{
            //    isCrouching = false;
            //    controller.enabled = false;
            //    transform.position += new Vector3(0, crouchHeight / 2);
            //    controller.enabled = true;
            //    controller.height = standingHeight;
            //    currentSpeed = crouchSpeed;
            //}
            //else
            //{
            //    isCrouching = true;
            //    controller.height = crouchHeight;
            //    controller.enabled = false;
            //    transform.position -= new Vector3(0, crouchHeight / 2);
            //    controller.enabled = true;
            //    currentSpeed = walkSpeed;
            //}

            uiManager.CrouchingImage(isCrouching);
        }
    }

    public void TeleportPlayer(Vector3 pos) //Respawn method, this method is here because it needs the controller, playervariables handles player death, this only teleports the player
    {
        currentSpeed = 0;
        controller.enabled = false;
        Debug.Log("Teleport activated on position " + pos);
        transform.position = pos;
        controller.enabled = true;
    }

    private void CheckGround()
    {
        //CheckSphere creates an overlap-check in the form of a sphere at a [1]position, with a [2]radius, that only detects objects(with collider) assigned with a specific [3]layer.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        //Reset gravity force when the character is grounded. This prevents gravity buildup.
        if (controller.isGrounded && yVelocity.y < 0)
            yVelocity.y = -2f;
    }

    //public static bool IsDodging() { return isDodging; }

    //public bool IsSprinting() { return isSprinting; }

    //public bool IsMoving() { return controller.isGrounded && inputX != 0 || controller.isGrounded && inputZ != 0; }

    public float GetHorizontalInput() { return inputX; }
    public float GetVerticalInput() { return inputZ; }
    public Vector3 GetDodgeDirection() { return dodgeDirection; }

    private void ApplyYAxisVelocity()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded && !IsDodging && playerVariables.GetCurrentStamina() > staminaUsedForJump)
        {
            playerVariables.StaminaToBeUsed(staminaUsedForJump);
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //This builds up the downward velocity vector with gravity.
        yVelocity.y += gravity * Time.deltaTime;

        //This updates the player's position with the downward velocity. This is multiplied by deltaTime again for the formula of real gravitation.
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
        Debug.Log("Time has restored to : " + Time.timeScale);
        audioManager.Play("SlowMoFinish");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(camHolderPositionOrigin, 0.6f);
    }
}
