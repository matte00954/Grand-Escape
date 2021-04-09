using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{

    public UiManager uiManager;

    [Header("Variables")]
    public float secondsBeforeStaminaRegen = 1f;
    public int staminaToBeGainedPerTick = 5;

    public int secondsBeforeStaminaLoss;
    public int staminaToBeLostPerTickOfSprint = 5;

    [Header("Max Variables")]
    public int maxHealthPoints;
    public int maxAmmo;
    public int maxStamina;

    [Header("Pickup Amount")]
    public int healthBoostAmount;
    public int ammoBoxAmount;

    private int healthPoints;
    private int currentAmmo;
    private int currentStamina;



    public int GetCurrentStamina()
    {
        return currentStamina;
    }

    public int GetCurrentHealthPoints()
    {
        return healthPoints;
    }

    public int GetCurrentTotalAmmo()
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

    public void ApplyDamage(int damageToBeApplied)
    {
        Debug.Log("Player took :" + damageToBeApplied + " damage");
        healthPoints -= damageToBeApplied;
        uiManager.HealthPoints(healthPoints);
    }

    private void Awake()
    {
        healthPoints = maxHealthPoints;
        currentAmmo = maxAmmo;
        currentStamina = maxStamina;

        uiManager.HealthPoints(healthPoints);
        uiManager.Stamina(currentStamina);
        uiManager.AmmoStatus(currentAmmo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Health boost") && healthPoints < maxHealthPoints)
        {
            healthPoints += healthBoostAmount;
            Destroy(other.gameObject);
            uiManager.HealthPoints(GetCurrentHealthPoints());
            Debug.Log("HP restored by " + healthBoostAmount);
        }
        if (other.gameObject.CompareTag("Ammo boost") && currentAmmo < maxAmmo)
        {
            currentAmmo += ammoBoxAmount;
            Destroy(other.gameObject);
            if(currentAmmo > maxAmmo)
            {
                currentAmmo = maxAmmo;
            }
            uiManager.AmmoStatus(GetCurrentTotalAmmo());
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
            Debug.Log("TEST: Removing 10HP/AMMO of each");

            healthPoints -= 10;
            currentAmmo -= 10;
        }

        if(healthPoints <= 0)
        {
            Debug.Log("PLAYER HAS DIED");
        }

        if(currentStamina <= maxStamina)
        {
            StartCoroutine(StaminaGain(staminaToBeGainedPerTick));
        }
    }

    public void StaminaToBeUsed()
    {
        StartCoroutine(StaminaLoss(staminaToBeLostPerTickOfSprint));
    }

    public IEnumerator StaminaGain(int staminaToBeGained) //�r public ifall att det beh�vs
    {
        yield return new WaitForSeconds(secondsBeforeStaminaRegen); //efter x sekunder s� f�r man stamina
        currentStamina += staminaToBeGainedPerTick;
        uiManager.Stamina(currentStamina);
    }

    public IEnumerator StaminaLoss(int staminaToBeUsed)
    {
        yield return new WaitForSeconds(secondsBeforeStaminaLoss); //efter x sekunder f�rlorar man stamina
        currentStamina -= staminaToBeLostPerTickOfSprint;
        uiManager.Stamina(currentStamina);
    }
}

