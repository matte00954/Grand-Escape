//Author: William Örnquist
using UnityEngine;

public class CamAnimation : MonoBehaviour
{
    private CharacterController controller;
    private PlayerMovement movement;
    private Animation anim; //Empty GameObject's animation component

    private bool left;
    private bool right;

    void CameraAnimations()
    {
        if (movement.GetHorizontalInput() != 0 || movement.GetVerticalInput() != 0)
            if (controller.isGrounded && !movement.IsDodging())
                PlayWalkAnimation();
        
        if (movement.IsDodging())
            PlayDodgeAnimation();
    }

    private void PlayWalkAnimation()
    {
        //Waits until no walk animation is playing to play the next
        if (!anim.isPlaying && left == true) 
        {
            if (movement.IsSprinting())
                anim.Play("sprintBobLeft");
            else
                anim.Play("walkBobLeft");

            left = false;
            right = true;
        }

        if (!anim.isPlaying && right == true)
        {
            if (movement.IsSprinting())
                anim.Play("sprintBobRight");
            else
                anim.Play("walkBobRight");

            right = false;
            left = true;
        }
    }

    private void PlayDodgeAnimation() 
    {
        if (movement.GetHorizontalInput() == -1)
            anim.Play("dodgeLeft");
        else if (movement.GetHorizontalInput() == 1)
            anim.Play("dodgeRight");
    }


    void Start()
    { //First step in a new scene/life/etc. will be "walkLeft"
        left = true;
        right = false;

        anim = GetComponent<Animation>();
        controller = GetComponentInParent<CharacterController>();
        movement = GetComponentInParent<PlayerMovement>();
    }


    void Update()
    {
        CameraAnimations();
    }
}