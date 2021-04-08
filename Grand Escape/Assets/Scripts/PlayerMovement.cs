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
    public float slowMotionTime;
    public float dodgeTimer;
    public float dodgeAmountOfTime;

    public float slowMotionAmountMultiplier;

    private bool sideWayDashAvailable = true;

    Vector3 velocity; //This vector is used for storing added gravity every frame, building up downward velocity.
    bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        //CheckSphere creates an overlap-check in the form of a sphere at a [1]position, with a [2]radius, that only detects objects(with collider) assigned with a specific [3]layer.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        //Reset gravity force when the character is grounded. This prevents gravity buildup.
        if (isGrounded && velocity.y < 0) 
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal"); //Horizontal: "D" returns 1, "A" returns -1. Moving sideways left/right.
        float z = Input.GetAxis("Vertical"); //Vertical: "W" returns 1, "S" returns -1. Moving forward/backward.

        //This is how we update the player's movement.
        //Creates a vector that decides the direction of the player's movement on X and Z axis. Moving sideways left means transform.right(1) multiplied by 'x'(-1) is -1. Vector is then (-1, 0, 0).
        Vector3 move = transform.right * x + transform.forward * z;

        //Equivalent to updating the player's position (position += ...), but with CharacterController. 
        controller.Move(move * speed * Time.deltaTime);

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

        //This builds up the downward velocity vector with gravity.
        velocity.y += gravity * Time.deltaTime;

        //This updates the player's position with the downward velocity. This is multiplied by deltaTime again for the formula of real gravitation.
        controller.Move(velocity * Time.deltaTime);


        if (sideWayDashAvailable)
        {
            if (Input.GetKeyDown("q"))
            {
                controller.Move(move * 500 * Time.deltaTime);
                StartCoroutine(SlowMotion());
            }

            if (Input.GetKeyDown("e"))
            {
                controller.Move(-move * 500 * Time.deltaTime);
                StartCoroutine(SlowMotion());
            }
        }
    }




    private IEnumerator SlowMotion()
    {
        Time.timeScale = slowMotionAmountMultiplier;
        yield return new WaitForSeconds(slowMotionTime * slowMotionAmountMultiplier);
        Time.timeScale = 1f; //återställer till vanlig tid
    }
}
