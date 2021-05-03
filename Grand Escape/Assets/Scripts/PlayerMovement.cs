using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] PlayerVariables playerVariables;
    [SerializeField] AudioManager audioManager;

    [SerializeField] UiManager uiManager;

    [SerializeField] Transform groundCheck; //The groundCheck object.

    [SerializeField] LayerMask groundMask;

    [SerializeField] float groundDistance = 0.4f; //The radius of the CheckSphere for 'groundCheck'.

    //[Header("Controller")]
    CharacterController controller; //Reference to the player's CharacterController component.

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;
    [SerializeField] float crouchHeight;
    [SerializeField] float sprintCooldown;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchSpeed;

    [Header("Stamina")]
    [SerializeField] float staminaUsedForSprint;
    [SerializeField] float staminaUsedForDodge;
    [SerializeField] float staminaUsedTimeSlow;
    [SerializeField] float staminaUsedForJump;
    [SerializeField] float sprintExhaustionTime;

    [Header("Slow motion")]
    [SerializeField] float slowMotionDelayTime; //Remove later
    [SerializeField] float slowMotionAmountMultiplier;
    [SerializeField] float slowMotionStaminaToBeUsedPerTick;
    [SerializeField] float slowMotionTickTime;
    [SerializeField] float slowMotionExhaustionTime;
    [SerializeField] float slowMotionMovementSpeed;

    [Header("Dodge")]
    [SerializeField] float dodgeAmountOfTime;
    [SerializeField] float dodgeSpeedMultiplier;

    Vector3 dodgeDirection;
    Vector3 velocity; //This vector is used for storing added gravity every frame, building up downward velocity


    bool isSprinting = false;
    bool isExhaustedFromSprinting = false;
    bool isDodging = false;
    bool isGrounded;
    bool isSlowmotion = false;
    bool breakSlowMotion = false;
    bool isCrouching = false;

    bool isExhaustedFromSlowMotion = false;

    float currentSpeed;
    float standingHeight;
    float inputX;
    float inputZ;

    //Timers
    float dodgeTimer, slowMotionExhaustionTimer, slowMotionTickTimer, sprintExhaustionTimer;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        standingHeight = controller.height;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if player is grounded and resets gravity velocity if true
        CheckGround();
        //WASD Input
        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * inputX + transform.forward * inputZ;

        //Applying WASD- or Dodge-movement based on 'Dodge'-state
        if (!isDodging)
            controller.Move(move * currentSpeed * Time.deltaTime);
        else if (isDodging && dodgeTimer < dodgeAmountOfTime)
            ApplyDodge();
        else
            isDodging = false;

        //Crouch
        Crouch();

        //Sprint
        Sprint();

        //Dodge activation
        Dodge();

        //Time slow activation
        CheckTimeSlow();

        //Apply gravity and jump velocity
        ApplyYAxisVelocity();
    }

    private void Sprint()
    {
        isSprinting = (!isDodging && !isCrouching && isGrounded && Input.GetKey(KeyCode.LeftShift) && playerVariables.GetCurrentStamina() > 0 &&
                    inputZ == 1 && inputX == 0 && !isExhaustedFromSprinting);

        if (isSprinting)
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
        else if (Input.GetKey(KeyCode.Z))
        {
            currentSpeed = crouchSpeed;
            Crouch();
        }
        else
            currentSpeed = speed;

        if (isExhaustedFromSprinting && sprintExhaustionTimer <= 0f)
        {
            isExhaustedFromSprinting = false;
            uiManager.SprintExhaustion(isExhaustedFromSprinting);
        }
        else if (isExhaustedFromSprinting && sprintExhaustionTimer > 0f)
            sprintExhaustionTimer -= Time.unscaledDeltaTime;
    }

    private void Dodge()
    {
        if (!isDodging && controller.isGrounded && Input.GetKeyDown(KeyCode.F) && playerVariables.GetCurrentStamina() > staminaUsedForDodge && inputX != 0)
        {
            isDodging = true;
            dodgeDirection = transform.right * inputX + transform.forward * inputZ * 0.5f;

            playerVariables.StaminaToBeUsed(staminaUsedForDodge);
            dodgeTimer = 0f;
        }
    }

    private void CheckTimeSlow()
    {
        if (isSlowmotion && Input.GetKeyDown(KeyCode.C) || isSlowmotion && playerVariables.GetCurrentStamina() <= 0f)
        {
            Debug.Log("Slow motion stops");
            isSlowmotion = false;
            isExhaustedFromSlowMotion = true;
            uiManager.SlowMotionExhaustion(isExhaustedFromSlowMotion);
            RestoreTime();
        }

        if (Input.GetKeyDown(KeyCode.C) && playerVariables.GetCurrentStamina() > staminaUsedTimeSlow && !isExhaustedFromSlowMotion && !isSlowmotion)
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

    private void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (isCrouching)
            {
                isCrouching = false;
                controller.enabled = false;
                transform.position += new Vector3(0, crouchHeight / 2);
                controller.enabled = true;
                controller.height = standingHeight;
            }
            else
            {
                isCrouching = true;
                controller.height = crouchHeight;
                controller.enabled = false;
                transform.position -= new Vector3(0, crouchHeight / 2);
                controller.enabled = true;
            }
        }
    }

    public void SetControllerInactive() //Dead state, should not be able to move
    {
        controller.enabled = false;
        currentSpeed = 0;
    }

    public void TeleportPlayer(Vector3 pos) //Respawn method, this method is here because it needs the controller, playervariables handles player death, this only teleports the player
    {
        Debug.Log("Teleport activated on position " + pos);
        transform.position = pos;
        controller.enabled = true;
    }

    private void CheckGround()
    {
        //CheckSphere creates an overlap-check in the form of a sphere at a [1]position, with a [2]radius, that only detects objects(with collider) assigned with a specific [3]layer.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Reset gravity force when the character is grounded. This prevents gravity buildup.
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;
    }

    private void ApplyDodge()
    {
        if (Time.timeScale < 1f)
            controller.Move((dodgeDirection * currentSpeed * dodgeSpeedMultiplier) * slowMotionAmountMultiplier * Time.deltaTime);
        else
            controller.Move(dodgeDirection * currentSpeed * dodgeSpeedMultiplier * Time.deltaTime);

        dodgeTimer += Time.deltaTime;
    }

    public bool IsDodging() { return isDodging; }

    public bool IsSprinting() { return isSprinting; }

    public bool IsWalking() { return controller.isGrounded && inputX != 0 || controller.isGrounded && inputZ != 0; }

    private void ApplyYAxisVelocity()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded && !isDodging && playerVariables.GetCurrentStamina() > staminaUsedForJump)
        {
            playerVariables.StaminaToBeUsed(staminaUsedForJump);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        //This builds up the downward velocity vector with gravity.
        velocity.y += gravity * Time.deltaTime;

        //This updates the player's position with the downward velocity. This is multiplied by deltaTime again for the formula of real gravitation.
        controller.Move(velocity * Time.deltaTime);
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
        currentSpeed = speed;
        Time.timeScale = 1f; //returns to normal time
        isSlowmotion = false;
        breakSlowMotion = false;
        Debug.Log("Time has restored to : " + Time.timeScale);
        audioManager.Play("SlowMoFinish");
    }
}
