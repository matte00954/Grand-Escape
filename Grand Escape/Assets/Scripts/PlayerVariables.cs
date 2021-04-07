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

    [HideInInspector]
    public int healthPoints;
    public int currentAmmo;

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public int SetCurrentAmmo(int sizeOfFullClipFromWeapon)
    {
        //parametern �r allts� vad vapnet som laddas om har f�r max kapacitet
        if(currentAmmo - sizeOfFullClipFromWeapon > 0) // kollar att man kan ladda hela clippet
        {
            Debug.Log("Loading full clip");
            currentAmmo -= sizeOfFullClipFromWeapon;
            return sizeOfFullClipFromWeapon; //du f�r fullt clip
        }
        else //du kan inte f� fullt klipp men du har n�gra kulor kvar
        {
            Debug.Log("Part of the clip is loaded");
            int ammoAmountToReturn = currentAmmo; //sparar m�ngden ammo som du laddar clippet med och skickar tillbaka detta
            currentAmmo = 0; //du laddar resten av all ammo du har
            return ammoAmountToReturn; //returnerar resten av all ammo som �r kvar, som laddas in
        }
    }

    public int GetCurrentHealthPoints()
    {
        return healthPoints;
    }

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
            Debug.Log("Ammo is " + currentAmmo);
        }

        if (Input.GetKeyDown("k")) //TEST
        {
            Debug.Log("Removing 10 of each");

            healthPoints -= 10;
            currentAmmo -= 10;
        }
    }
}
