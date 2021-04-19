using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] GameObject meeleBox;
    [SerializeField] float meeleCooldown;
    [SerializeField] float meeleAttackActive;
    [SerializeField] float meeleStaminaCost = 5;

    AudioManager audioManager;

    PlayerVariables playerVariables;

    private void Awake()
    {
        playerVariables = GetComponent<PlayerVariables>();
    }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private bool meeleAttackAvailable = true;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V) && meeleAttackAvailable && playerVariables.GetCurrentStamina() > meeleStaminaCost)
        {
            Debug.Log("Player performs melee attack");
            playerVariables.StaminaToBeUsed(meeleStaminaCost);
            meeleAttackAvailable = false;
            meeleBox.SetActive(true);
            StartCoroutine(MeeleCooldown());
        }
    }

    private IEnumerator MeeleCooldown()
    {
        audioManager.Play("DrawSword");
        yield return new WaitForSeconds(meeleAttackActive); //hur länge tills meele boxen försvinner
        audioManager.Play("SwingSword");
        meeleBox.SetActive(false);
        yield return new WaitForSeconds(meeleCooldown); //hur länge tills meele attack går att göra igen
        meeleAttackAvailable = true;
    }
}
