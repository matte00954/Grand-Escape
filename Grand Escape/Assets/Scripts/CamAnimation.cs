//Author: William Örnquist
using UnityEngine;

public class CamAnimation : MonoBehaviour
{
    private AudioManager audioManager;
    private Animator animator;

    private readonly string isMovingParameterName = "IsMoving";
    private readonly string isSprintingParameterName = "IsSprinting";
    private readonly string isDodgingParameterName = "IsDodging";
    private readonly string playerDiedParameterName = "PlayerDied";
    private readonly string isDodgingLeftParameterName = "IsDodgingLeft";
    private readonly string isDodgingRightParameterName = "IsDodgingRight";
    private readonly string isAliveParameterName = "IsAlive";

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        animator = GetComponent<Animator>();
    }

    private void Update() => UpdateAnimatorParameters();

    private void UpdateAnimatorParameters()
    {
        animator.SetBool(isMovingParameterName, PlayerMovement.IsMoving);
        animator.SetBool(isSprintingParameterName, PlayerMovement.IsSprinting);
        animator.SetBool(isDodgingParameterName, PlayerMovement.IsDodging);
        animator.SetBool(isAliveParameterName, PlayerVariables.isAlive);
    }

    public void PlayDeathAnimation() => animator.SetTrigger(playerDiedParameterName);
    public void PlayDodgeLeft() => animator.SetTrigger(isDodgingLeftParameterName);
    public void PlayDodgeRight() => animator.SetTrigger(isDodgingRightParameterName);
    public void CallAnimationSFX(string name) //This method is only called from animation events.
    {
        if (name is null)
        {
            Debug.LogError("CamAnimation 'CallFootstepSFX' recieved null.");
            return;
        }

        audioManager.Play(name);
    }
}