using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public Camera playerCamera;
    public GameObject ammo;

    private GameObject player;
    private RaycastHit shootHit;
    private Ray playerAim;

    public int ammoCapacity; //hur många skott i vapnet

    private bool isReloading;
    private int currentAmmo; //skott som är laddade

    private void Awake()
    {
        isReloading = false;
        player = this.gameObject;
        currentAmmo = ammoCapacity;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 direction = player.transform.position - point;

        playerAim = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            currentAmmo--;
            Debug.Log("Current ammo: " + currentAmmo);
            Debug.Log("Shot");
            Instantiate(ammo, point, Quaternion.identity);
            if (Physics.Raycast(playerAim, out shootHit))
            {
                Transform objectHit = shootHit.transform;

                //Debug.Log("Hit Object: " + objectHit);
            }
            //Debug.DrawRay(point, direction, Color.red); //denna funkar ej
        }


        if (Input.GetKeyDown("r") && !isReloading && currentAmmo == 0)
        {
            isReloading = true;
            Debug.Log("Reloading...");
            StartCoroutine(Reloading());
        }
    }

    IEnumerator Reloading()
    {
        Debug.Log("3");
        yield return new WaitForSeconds(1f);
        Debug.Log("2");
        yield return new WaitForSeconds(1f);
        Debug.Log("1");
        yield return new WaitForSeconds(1f);
        Debug.Log("Reloaded");
        currentAmmo = ammoCapacity;
        isReloading = false;
    }
}
