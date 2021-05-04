using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] MeshCollider meleeCollider;
    //[SerializeField] float meleeCooldown;
    //[SerializeField] float meleeAttackActive;
    [SerializeField] float meleeStaminaCost = 5;

    AudioManager audioManager;

    PlayerVariables playerVariables;

    bool meleeIsActive = false;

    private void Awake()
    {
        playerVariables = GetComponent<PlayerVariables>();
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        meleeCollider.enabled = false;
    }

    //private bool meeleAttackAvailable = true;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && !meleeIsActive && playerVariables.GetCurrentStamina() > meleeStaminaCost)
        {
            Debug.Log("Player performs melee attack");
            playerVariables.StaminaToBeUsed(meleeStaminaCost);
            //meeleAttackAvailable = false;
            //meleeCollider.SetActive(true);
            //StartCoroutine(MeeleCooldown());
        }
    }

    //private IEnumerator MeeleCooldown()
    //{
    //    audioManager.Play("DrawSword");
    //    yield return new WaitForSeconds(meleeAttackActive); //hur länge tills meele boxen försvinner
    //    audioManager.Play("SwingSword");
    //    meeleBox.SetActive(false);
    //    yield return new WaitForSeconds(meleeCooldown); //hur länge tills meele attack går att göra igen
    //    meeleAttackAvailable = true;
    //}
}
