using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] PlayerVariables playerVariables;

    [SerializeField] Transform groundCheck; //The groundCheck object.

    [SerializeField] LayerMask groundMask;

    [SerializeField] float groundDistance = 0.4f; //The radius of the CheckSphere for 'groundCheck'.

    [Header("Controller")]
    [SerializeField] CharacterController controller; //Reference to the player's CharacterController component.

    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float gravity;
    [SerializeField] float jumpHeight;
    [SerializeField] float sprintCooldown;
    [SerializeField] float sprintSpeed;
    [SerializeField] float crouchSpeed;

    [Header("Stamina")]
    [SerializeField] float staminaUsedForSprint;
    [SerializeField] float staminaUsedForDodge;
    [SerializeField] float staminaUsedTimeSlow;
    [SerializeField] float staminaUsedForJump;
    [SerializeField] float ranOutOfStaminaTimer;

    [Header("Slow motion")]
    [SerializeField] float slowMotionTime;
    [SerializeField] float slowMotionDelay;
    [SerializeField] float slowMotionAmountMultiplier;
    [SerializeField] float slowMotionStaminaToBeUsedPerTick;
    [SerializeField] float slowMotionTick;
    [SerializeField] float exhaustedFromSlowMotionTimer;

    [Header("Dodge")]
    [SerializeField] float dodgeAmountOfTime;
    [SerializeField] float dodgeSpeedMultiplier;

     Vector3 dodgeDirection;
     Vector3 velocity; //This vector is used for storing added gravity every frame, building up downward velocity

    bool isSprinting = false;
    bool ranOutOfStaminaAndCanNotSprint = false;
    bool isDodging = false;
    bool isGrounded;
    bool isSlowmotion = false;
    bool breakSlowMotion = false;

    bool exhaustedFromSlowMotion = false;

    float currentSpeed;
    float dodgeTimer = 0f; //Needs to be zero
    float inputX;
    float inputZ;

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


        if (isSlowmotion && Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Break!!!");
            breakSlowMotion = true;
        }

        //Sprint
        if (!isDodging && isGrounded && Input.GetKey(KeyCode.LeftShift) && playerVariables.GetCurrentStamina() > 0 &&
            inputZ == 1 && inputX == 0 && !ranOutOfStaminaAndCanNotSprint)
        {
            isSprinting = true;
        }
        else
            isSprinting = false;

        //Dodge activation
        if (!isDodging && isGrounded && Input.GetKeyDown(KeyCode.F) && playerVariables.GetCurrentStamina() > staminaUsedForDodge && inputX != 0)
        {
            isDodging = true;
            dodgeDirection = transform.right * inputX + transform.forward * inputZ * 0.5f;

            playerVariables.StaminaToBeUsed(staminaUsedForDodge);
            dodgeTimer = 0f;
            //StartCoroutine(SlowMotion());
        }

        //Time slow activation
        if (Input.GetKeyDown(KeyCode.C) && playerVariables.GetCurrentStamina() > staminaUsedTimeSlow && !exhaustedFromSlowMotion && !isSlowmotion)
        {
            playerVariables.StaminaToBeUsed(staminaUsedTimeSlow);
            StartCoroutine(SlowMotion());
        }

        if (isSprinting)
        {
            currentSpeed = sprintSpeed;
            playerVariables.StaminaToBeUsed(staminaUsedForSprint);

            if(playerVariables.GetCurrentStamina() < 1f)
            {
                StartCoroutine(ExhaustedFromSprinting());
            }
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            currentSpeed = crouchSpeed;
            Crouch();
        }
        else
        {
            currentSpeed = speed;
        }

        //Apply gravity and jump velocity
        ApplyYAxisVelocity();
    }

    private void Crouch()
    {
        //TODO
    }

    public void TeleportPlayer(Vector3 pos)
    {
        controller.enabled = false;
        Debug.Log("Teleport" + pos);
        transform.position = pos;
        controller.enabled = true;
    }

    private void CheckGround()
    {
        //CheckSphere creates an overlap-check in the form of a sphere at a [1]position, with a [2]radius, that only detects objects(with collider) assigned with a specific [3]layer.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Reset gravity force when the character is grounded. This prevents gravity buildup.
        if (isGrounded && velocity.y < 0)
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

    public bool IsWalking() { return isGrounded && inputX != 0 || isGrounded && inputZ != 0; }

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

    private IEnumerator SlowMotion()
    {
        yield return new WaitForSeconds(slowMotionDelay);
        isSlowmotion = true;
        Time.timeScale = slowMotionAmountMultiplier;

        while (playerVariables.GetCurrentStamina() > slowMotionStaminaToBeUsedPerTick)
        {
            if (breakSlowMotion)
            {
                break;
            }
            yield return new WaitForSeconds(slowMotionTick);
            playerVariables.StaminaToBeUsed(slowMotionStaminaToBeUsedPerTick);
        }
        StartCoroutine(ExhaustedFromSlowMotion());
        RestoreTime();
    }

    private void RestoreTime()
    {
        Time.timeScale = 1f; //returns to normal time
        isSlowmotion = false;
        breakSlowMotion = false;
    }

    private IEnumerator ExhaustedFromSlowMotion()
    {
        exhaustedFromSlowMotion = true;
        Debug.Log("Player exhausted and can not slow mo " + exhaustedFromSlowMotionTimer + " seconds");
        yield return new WaitForSeconds(exhaustedFromSlowMotionTimer); //Player gets exhausted and cant slow mo for this amount of time
        exhaustedFromSlowMotion = false;
    }

    private IEnumerator ExhaustedFromSprinting()
    {
        ranOutOfStaminaAndCanNotSprint = true;
        Debug.Log("Player exhausted and can not sprint for " + ranOutOfStaminaTimer + " seconds");
        yield return new WaitForSeconds(ranOutOfStaminaTimer); //Player gets exhausted and cant sprint for this amount of time
        ranOutOfStaminaAndCanNotSprint = false;
    }
}
