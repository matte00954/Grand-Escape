using UnityEngine;
using System.Collections;

public class CamAnimation : MonoBehaviour
{
    [SerializeField] CharacterController playerController;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] Animation anim; //Empty GameObject's animation component

    private bool isMoving;

    private bool left;
    private bool right;
    private float inputX;
    private float inputY;

    void CameraAnimations()
    {
        if (inputX != 0 || inputY != 0)
            isMoving = true;
        else if (inputX == 0 && inputY == 0)
            isMoving = false;

        if (playerController.isGrounded && isMoving && !playerMovement.IsDodging())
            PlayWalkAnim();

        if (playerMovement.IsDodging())
            PlayDodgeAnim();
    }

    private void PlayWalkAnim()
    {
        if (!anim.isPlaying && left == true)
        {//Waits until no animation is playing to play the next
            if (playerMovement.IsSprinting())
                anim.Play("sprintBobLeft");
            else
                anim.Play("walkBobLeft");

            left = false;
            right = true;
        }

        if (!anim.isPlaying && right == true)
        {
            if (playerMovement.IsSprinting())
                anim.Play("sprintBobRight");
            else
                anim.Play("walkBobRight");

            right = false;
            left = true;
        }
    }

    private void PlayDodgeAnim() 
    {
        if (inputX == -1)
            anim.Play("dodgeLeft");
        else if (inputX == 1)
            anim.Play("dodgeRight");
    }


    void Start()
    { //First step in a new scene/life/etc. will be "walkLeft"
        left = true;
        right = false;
    }


    void Update()
    {
        inputX = Input.GetAxis("Horizontal"); //Keyboard input to determine if player is moving
        inputY = Input.GetAxis("Vertical");

        CameraAnimations();
    }
}