using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private MeshCollider meleeCollider;
    [SerializeField] private float meleeStaminaCost = 5;

    private AudioManager audioManager;
    private PlayerVariables playerVariables;

    private bool meleeIsActive = false;

    private void Awake() => playerVariables = GetComponent<PlayerVariables>();

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        meleeCollider.enabled = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && !meleeIsActive && playerVariables.GetCurrentStamina() > meleeStaminaCost)
        {
            Debug.Log("Player performs melee attack");
            playerVariables.StaminaToBeUsed(meleeStaminaCost);
        }
    }
}
