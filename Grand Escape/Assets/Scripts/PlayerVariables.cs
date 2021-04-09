using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{

    public UiManager uiManager;

    [Header("Stamina")]
    public int staminaRegenPerTick;
    public float timeUntilStaminaRegen;

    [Header("Max Variables")]
    public int maxHealthPoints;
    public int maxAmmo;
    public float maxStamina;

    [Header("Pickup Amount")]
    public int healthBoostAmount;
    public int ammoBoxAmount;

    private int healthPoints;
    private int currentAmmo;
    private float currentStamina;



    public float GetCurrentStamina()
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
        //parametern är alltså vad vapnet som laddas om har för max kapacitet
        if(currentAmmo - sizeOfFullClipFromWeapon > 0) // kollar att man kan ladda hela clippet
        {
            Debug.Log("Loading full clip");
            currentAmmo -= sizeOfFullClipFromWeapon;
            return sizeOfFullClipFromWeapon; //du får fullt clip
        }
        else //du kan inte få fullt klipp men du har några kulor kvar
        {
            Debug.Log("Part of the clip is loaded");
            int ammoAmountToReturn = currentAmmo; //sparar mängden ammo som du laddar clippet med och skickar tillbaka detta
            currentAmmo = 0; //du laddar resten av all ammo du har
            return ammoAmountToReturn; //returnerar resten av all ammo som är kvar, som laddas in
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
        uiManager.Stamina((int)currentStamina);
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


        if(currentStamina < maxStamina)
        {
            StartCoroutine(StaminaRegen(currentStamina));
        }

        /*if (currentStamina <= 0)
        {
            StaminaToBeGained();
        }*/
    }

    public void StaminaToBeUsed(float amount)
    {

        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            uiManager.Stamina((int)currentStamina);
        }
        else
            Debug.Log("Out of stamina");
    }
    public IEnumerator StaminaRegen(float oldCurrentStamina)
    {

        yield return new WaitForSeconds(timeUntilStaminaRegen);

        if (oldCurrentStamina == currentStamina)
        {
            currentStamina += staminaRegenPerTick;
            if(currentStamina > maxStamina) //om stamina går över max värdet, blir currentStamina istället max
            {
                currentStamina = maxStamina;
            }
            uiManager.Stamina((int)currentStamina);
        }
    }


    /*public IEnumerator StaminaGain(int staminaToBeGained) //är public ifall att det behövs
    {
        yield return new WaitForSeconds(secondsBeforeStaminaRegen); //efter x sekunder så får man stamina
        currentStamina += staminaToBeGainedPerTick;
        uiManager.Stamina(currentStamina);
    }*/

    /*public IEnumerator StaminaLoss(int staminaToBeUsed)
    {
        yield return new WaitForSeconds(secondsBeforeStaminaLoss); //efter x sekunder förlorar man stamina
        currentStamina -= staminaToBeLostPerTickOfSprint;
        uiManager.Stamina(currentStamina);
    }*/
}

