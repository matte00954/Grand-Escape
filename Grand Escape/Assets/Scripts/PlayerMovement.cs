using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller; //Reference to the player's CharacterController component.
    public float speed = 12f; //Player's movement speed.
    public float gravity = -9.81f; //Gravity increase rate.
    public float jumpHeight = 3f;

    public Transform groundCheck; //The groundCheck object.
    public float groundDistance = 0.4f; //The radius of the CheckSphere for 'groundCheck'.
    public LayerMask groundMask;

    [Header("Slow motion dash")]
    private Vector3 dodgeDirection;
    public float slowMotionTime = 1f;
    private float dodgeTimer = 0f;
    public float dodgeAmountOfTime = 0.7f;
    public float dodgeSpeedMultiplier = 3f;
    public float slowMotionDelay = 0.25f;
    public float slowMotionAmountMultiplier;
    private bool isDodging = false;



    Vector3 velocity; //This vector is used for storing added gravity every frame, building up downward velocity
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        //Checks if player is grounded and resets gravity velocity if true
        CheckGround();

        //WASD Input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        //Dodge activation
        if (!isDodging && isGrounded && Input.GetKeyDown(KeyCode.F))
        {
            isDodging = true;
            dodgeDirection = move;
            dodgeTimer = 0f;
            StartCoroutine(SlowMotion());
        }
        
        //Applying WASD- or Dodge-movement based on 'Dodge'-state
        if (!isDodging)
            controller.Move(move * speed * Time.deltaTime);
        else if (isDodging && dodgeTimer < dodgeAmountOfTime)
            ApplyDodge();
        else
            isDodging = false;

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
            controller.Move((dodgeDirection * speed * dodgeSpeedMultiplier) * slowMotionAmountMultiplier * Time.deltaTime);
        else
            controller.Move(dodgeDirection * speed * dodgeSpeedMultiplier * Time.deltaTime);

        dodgeTimer += Time.deltaTime;
    }

    private void ApplyYAxisVelocity()
    {
        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded && !isDodging)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

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
        Time.timeScale = 1f; //�terst�ller till vanlig tid
    }
}
