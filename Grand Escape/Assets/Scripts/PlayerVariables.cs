using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    [Header("Max Variables")]
    public int maxHealthPoints;
    public int maxAmmo;

    [Header("Pickup Amount")]
    public int healthBoostAmount;
    public int ammoBoxAmount;

    private int healthPoints;
    private int currentAmmo;

    private void Awake()
    {
        healthPoints = maxHealthPoints;
        currentAmmo = maxAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Health boost") && healthPoints < maxHealthPoints)
        {
            healthPoints += healthBoostAmount;
            Destroy(other.gameObject);
            Debug.Log("HP restored by " + healthBoostAmount);
        }
        if (other.gameObject.CompareTag("Ammo boost") && currentAmmo < maxAmmo)
        {
            currentAmmo += ammoBoxAmount;
            Destroy(other.gameObject);
            Debug.Log("Ammo restored by " + ammoBoxAmount);
        }

        if(healthPoints > maxHealthPoints)
        {
            Debug.Log("HP at max!");
            healthPoints = maxHealthPoints;
        }

        if(currentAmmo > maxAmmo)
        {
            Debug.Log("Ammo at max!");
            currentAmmo = maxAmmo;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown("g")) //TEST
        {
            Debug.Log("HP is " + healthPoints);
            Debug.Log("HP is " + currentAmmo);
        }

        if (Input.GetKeyDown("k")) //TEST
        {
            Debug.Log("Removing 10 of each");

            healthPoints -= 10;
            currentAmmo -= 10;
        }
    }
}
