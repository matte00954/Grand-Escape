using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [Header("Gameobjects")]
    public Camera playerCamera;
    public GameObject ammo;
    public PlayerVariables playerVariables;

    //private GameObject player; //kanske behövs i framtiden?
    private RaycastHit shootHit;
    private Ray playerAim;

    [Header("Ammo")]
    public int ammoCapacity; //hur många skott i vapnet

    private bool isReloading;
    private int currentAmmoLoaded; //skott som är laddade
    private int totalAmmoToReloadWith;

    private void Awake()
    {
        isReloading = false;
        currentAmmoLoaded = ammoCapacity;

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
            Debug.Log("Loaded: " + currentAmmoLoaded);
            Debug.Log("Total Ammo: " + playerVariables.GetCurrentAmmo());
            Debug.Log("Shot");
            Instantiate(ammo, point, Quaternion.identity);
            if (Physics.Raycast(playerAim, out shootHit))
            {
                Transform objectHit = shootHit.transform;

                //Debug.Log("Hit Object: " + objectHit);
            }
            //Debug.DrawRay(point, direction, Color.red); //denna funkar ej
        }

        if (Input.GetKeyDown("r") && !isReloading)
        {
            if (currentAmmoLoaded == 0 && playerVariables.GetCurrentAmmo() > 0)
            {
                isReloading = true;
                Debug.Log("Reloading...");
                StartCoroutine(Reloading());
            }
            else if (playerVariables.GetCurrentAmmo() == 0)
            {
                Debug.Log("No ammo left");
            }
            else if (playerVariables.GetCurrentAmmo() < 0)
            {
                Debug.Log("ERROR: CURRENT AMMO IS LOWER THAN ZERO");
            }
        }
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(3f);
        Debug.Log("Reloaded");
        currentAmmoLoaded = playerVariables.SetCurrentAmmo(ammoCapacity);
        isReloading = false;
    }
}
