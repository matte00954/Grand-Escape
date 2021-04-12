using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Gameobjects")]
    [SerializeField] Camera playerCamera;
    [SerializeField] GameObject ammo;
    [SerializeField] PlayerVariables playerVariables;
    [SerializeField] UiManager uiManager;

    //private GameObject player; //kanske behövs i framtiden?
    RaycastHit shootHit;
    Ray playerAim;

    [Header("Ammo")]
    [SerializeField] int ammoCapacity; //hur många skott i vapnet

    private bool isReloading;
    private int currentAmmoLoaded; //skott som är laddade
    private int totalAmmoToReloadWith;

    private void Awake()
    {
        isReloading = false;
        currentAmmoLoaded = ammoCapacity;

        uiManager.AmmoStatus(playerVariables.GetCurrentTotalAmmo());
        uiManager.WeaponStatus("LOADED");
        //player = this.gameObject; //ta inte bort, om den behövs i framtiden
    }


    // Update is called once per frame
    void Update()
    {
        Vector3 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);

        //Vector3 direction = player.transform.position - point; //används inte just nu, men kanske behövs i framtiden

        playerAim = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && currentAmmoLoaded > 0)
        {
            currentAmmoLoaded--;
            uiManager.WeaponStatus("Empty");
            Instantiate(ammo, point, playerCamera.transform.rotation);
            if (Physics.Raycast(playerAim, out shootHit)) //tror att detta ej används
            {
                Transform objectHit = shootHit.transform;

                //Debug.Log("Hit Object: " + objectHit);
            }
            //Debug.DrawRay(point, direction, Color.red); //denna funkar ej
        }

        if (Input.GetKeyDown("r") && !isReloading)
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
        yield return new WaitForSeconds(3f);
        uiManager.WeaponStatus("Reloaded");
        currentAmmoLoaded = playerVariables.SetCurrentAmmo(ammoCapacity);
        isReloading = false;
    }
}
