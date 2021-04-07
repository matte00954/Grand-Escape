using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public Camera playerCamera;
    public GameObject ammo;

    public int ammoCapacity;

    private GameObject player;
    private RaycastHit shootHit;
    private Ray playerAim;

    private void Awake()
    {
        player = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);
        //kanske ej behövs

        Vector3 direction = player.transform.position - point;
        //kanske ej behövs

        playerAim = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && Physics.Raycast(playerAim, out shootHit))
        {
            Transform objectHit = shootHit.transform;
            Vector3 impact = playerAim.GetPoint(0.1f);
            Instantiate(ammo, new Vector3(impact.x,impact.z,impact.z), Quaternion.identity);
            Debug.Log(objectHit);
            Debug.Log("Shot");
        }
    }
}
