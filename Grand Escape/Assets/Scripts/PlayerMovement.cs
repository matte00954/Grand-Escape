using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController controller; //Reference to the player's CharacterController component.
    public float speed = 10f; //Player's default movement speed.
    public float currentSpeed; //Player's current movement speed.
    public float gravity = -9.81f; //Gravity increase rate.
    public float jumpHeight = 3f;
    public float sprintCooldown;
    public float sprintSpeed = 18f;

    private bool isSprinting = false;

    [Header("Stamina")]
    public float staminaUsedForSprint;
    public float staminaUsedForDodge;
    public float staminaUsedTimeSlow;
    public float staminaUsedForJump;
    public float ranOutOfStaminaTimer;

    private bool ranOutOfStaminaAndCanNotSprint;

    [Header("Ground")]
    public Transform groundCheck; //The groundCheck object.
    public float groundDistance = 0.4f; //The radius of the CheckSphere for 'groundCheck'.
    public LayerMask groundMask;

    [Header("Slow motion dash")]
    public float slowMotionTime = 1f;
    public float slowMotionDelay = 0.25f;
    public float slowMotionAmountMultiplier;

    [Header("Dodge")]
    public float dodgeAmountOfTime = 0.7f;
    public float dodgeSpeedMultiplier = 3f;
    private bool isDodging = false;
    private Vector3 dodgeDirection;
    private float dodgeTimer = 0f;


    public PlayerVariables playerVariables;



    Vector3 velocity; //This vector is used for storing added gravity every frame, building up downward velocity
    bool isGrounded;

    private void Awake()
    {
        ranOutOfStaminaAndCanNotSprint = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Checks if player is grounded and resets gravity velocity if true
        CheckGround();

        //WASD Input
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        Vector3 move = transform.right * inputX + transform.forward * inputZ;

        //Applying WASD- or Dodge-movement based on 'Dodge'-state
        if (!isDodging)
            controller.Move(move * currentSpeed * Time.deltaTime);
        else if (isDodging && dodgeTimer < dodgeAmountOfTime)
            ApplyDodge();
        else
            isDodging = false;

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
        if (Input.GetKeyDown(KeyCode.C) && playerVariables.GetCurrentStamina() > staminaUsedTimeSlow)
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
                StartCoroutine(RanOutOfStamina());
            }
        }
        else
        {
            currentSpeed = speed;
        }

        //Apply gravity and jump velocity
        ApplyYAxisVelocity();
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
        Time.timeScale = slowMotionAmountMultiplier;
        yield return new WaitForSeconds(slowMotionTime * slowMotionAmountMultiplier);
        Time.timeScale = 1f; //återställer till vanlig tid
    }
    private IEnumerator RanOutOfStamina()
    {
        ranOutOfStaminaAndCanNotSprint = true;
        Debug.Log("Player exhausted and can not sprint for " + ranOutOfStaminaTimer + " seconds");
        yield return new WaitForSeconds(ranOutOfStaminaTimer); //Player gets exhausted and cant sprint for this amount of time
        ranOutOfStaminaAndCanNotSprint = false;
    }
}
