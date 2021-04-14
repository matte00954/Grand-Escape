using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Gameobjects")]
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject ammo;
    [SerializeField] PlayerVariables playerVariables;
    [SerializeField] CharacterController charController;
    [SerializeField] UiManager uiManager;
    [SerializeField] AudioManager audioManager;

    Animator animator;

    //private GameObject player; //kanske behövs i framtiden?
    RaycastHit shootHit;
    Ray playerAim;

    [Header("Ammo")]
    [SerializeField] int ammoCapacity; //hur många skott i vapnet
    [SerializeField] float reloadTime;

    private bool isReloading;
    private int currentAmmoLoaded; //skott som är laddade
    private int totalAmmoToReloadWith;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        isReloading = false;
        currentAmmoLoaded = ammoCapacity;

        uiManager.AmmoStatus(playerVariables.GetCurrentTotalAmmo());
        uiManager.WeaponStatus("LOADED");
        //player = this.gameObject; //ta inte bort, om den behövs i framtiden
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        animator.SetBool("Moving", (inputX != 0 && charController.isGrounded || inputZ != 0 && charController.isGrounded));

        Vector3 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 direction = player.transform.position - point; //används inte just nu, men kanske behövs i framtiden

        playerAim = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && currentAmmoLoaded > 0)
        {
            currentAmmoLoaded--;
            uiManager.WeaponStatus("Empty");
            Instantiate(ammo, point, playerCamera.transform.rotation);
            animator.SetTrigger("Fire");
            audioManager.Play("PistolShot");
            if (Physics.Raycast(playerAim, out shootHit)) //tror att detta ej används
            {
                Transform objectHit = shootHit.transform;

                //Debug.Log("Hit Object: " + objectHit);
            }
            //Debug.DrawRay(point, direction, Color.red); //denna funkar ej
        }

        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            uiManager.AmmoStatus(playerVariables.GetCurrentTotalAmmo());
            if (currentAmmoLoaded == 0 && playerVariables.GetCurrentTotalAmmo() > 0)
            {
                isReloading = true;
                StartCoroutine(Reloading());
            }
            else if (playerVariables.GetCurrentTotalAmmo() == 0)
            {
                Debug.Log("No ammo left");
            }
            else if (playerVariables.GetCurrentTotalAmmo() < 0)
            {
                Debug.LogError("ERROR: CURRENT AMMO IS LOWER THAN ZERO");
            }
        }
    }

    IEnumerator Reloading()
    {
        uiManager.WeaponStatus("Reloading...");
        animator.SetTrigger("Reload");
        yield return new WaitForSeconds(reloadTime);
        uiManager.WeaponStatus("Reloaded");
        animator.SetTrigger("FinishReload");
        currentAmmoLoaded = playerVariables.SetCurrentAmmo(ammoCapacity);
        isReloading = false;
    }
}
