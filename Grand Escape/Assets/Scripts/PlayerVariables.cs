using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{

    [SerializeField] UiManager uiManager;

    [SerializeField] GameObject respawnPoint;

    Vector3 currentRespawnPoint;

    GameObject Player;

    [Header("Stamina")]
    [SerializeField] int staminaRegenPerTick;
    [SerializeField] float timeUntilStaminaRegen;

    [Header("Max Variables")]
    [SerializeField] int maxHealthPoints;
    [SerializeField] int maxAmmo;
    [SerializeField] float maxStamina;

    [Header("Pickup Amount")]
    [SerializeField] int healthBoostAmount;
    [SerializeField] int ammoBoxAmount;
    [SerializeField] int staminaPickUpRestoreAmount;

    [Header("Damage Variables")]
    [SerializeField] float recentDamageTimer;

    int healthPoints;
    int currentAmmo;
    float currentStamina;
    bool takenRecentDamage = false;

    private void Awake()
    {
        Player = this.gameObject;
        healthPoints = maxHealthPoints;
        currentAmmo = maxAmmo;
        currentStamina = maxStamina;

        currentRespawnPoint = respawnPoint.transform.position;

        uiManager.HealthPoints(healthPoints);
        uiManager.Stamina((int)currentStamina);
        uiManager.AmmoStatus(currentAmmo);
    }


    public float GetCurrentStamina() { return currentStamina; }

    public int GetCurrentHealthPoints() { return healthPoints; }

    public int GetCurrentTotalAmmo() { return currentAmmo; }

    public float GetMaxStamina() { return maxStamina; }

    public float GetMaxAmmo() { return maxAmmo; }

    public float GetMaxHealth() { return maxHealthPoints; }

    public int SetCurrentAmmo(int sizeOfFullClipFromWeapon)
    {
        //parametern är alltså vad vapnet som laddas om har för max kapacitet
        if(currentAmmo - sizeOfFullClipFromWeapon > 0) // kollar att man kan ladda hela clippet
        {
            //Debug.Log("Loading full clip");
            currentAmmo -= sizeOfFullClipFromWeapon;
            return sizeOfFullClipFromWeapon; //du får fullt clip
        }
        else //du kan inte få fullt klipp men du har några kulor kvar
        {
            //Debug.Log("Part of the clip is loaded");
            Debug.LogError("ERROR: SetCurrentAmmo Else statement should not happen (unless weapon is semi-auto)");
            int ammoAmountToReturn = currentAmmo; //sparar mängden ammo som du laddar clippet med och skickar tillbaka detta
            currentAmmo = 0; //du laddar resten av all ammo du har
            return ammoAmountToReturn; //returnerar resten av all ammo som är kvar, som laddas in
        }
    }

    public void ApplyDamage(int damageToBeApplied)
    {
        Debug.Log("Player took :" + damageToBeApplied + " damage");
        if (!takenRecentDamage)
        {
            healthPoints -= damageToBeApplied;
            StartCoroutine(ResetTakenRecentDamage());
        }
        uiManager.HealthPoints(healthPoints);
    }

    public void ResetAllStats()
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

            if (healthPoints > maxHealthPoints)
            {
                Debug.Log("HP restored to max");
                healthPoints = maxHealthPoints;
            }
            else
                Debug.Log("HP restored by " + healthBoostAmount);

            uiManager.HealthPoints(healthPoints);
        }
        if (other.gameObject.CompareTag("Ammo boost") && currentAmmo < maxAmmo)
        {
            currentAmmo += ammoBoxAmount;
            Destroy(other.gameObject);

            if (currentAmmo > maxAmmo)
            {
                Debug.Log("Ammo restored to max");
                currentAmmo = maxAmmo;
            }
            else
                Debug.Log("Ammo restored by " + ammoBoxAmount);

            uiManager.AmmoStatus(GetCurrentTotalAmmo());
        }

        if (other.gameObject.CompareTag("Stamina boost") && currentStamina < maxStamina)
        {
            currentStamina += staminaPickUpRestoreAmount;
            Destroy(other.gameObject);

            if (currentStamina > maxStamina)
            {
                Debug.Log("Stamina restored to max");
                currentStamina = maxStamina;
            }
            else
                Debug.Log("Stamina restored by " + staminaPickUpRestoreAmount);

            uiManager.Stamina((int)currentStamina);
        }
    }
    private void Update()
    {
        if(healthPoints <= 0)
        {
            Debug.Log("PLAYER HAS DIED");
            PlayerRespawn();
        }

        if(currentStamina < maxStamina) //stamina regen start
        {
            StartCoroutine(StaminaRegen(currentStamina));
            uiManager.Stamina((int)currentStamina);
        }
    }

    private void PlayerRespawn()
    {
        PlayerMovement pm = GetComponent<PlayerMovement>();
        pm.TeleportPlayer(currentRespawnPoint);
        ResetAllStats();
    }

    private void SetNewRespawnPoint(Vector3 newRespawnPoint) //OBS används ej för tillfället
    {
        currentRespawnPoint = newRespawnPoint;
    }

    public void StaminaToBeUsed(float amount) //Everything that costs stamina should use this method
    {
        if (currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            uiManager.Stamina((int)currentStamina);
        }
        else 
            Debug.Log("Out of stamina");
    }

    private IEnumerator ResetTakenRecentDamage() //To prevent player from taking damage from the same bullet twice and to prevent too fast deaths
    {
        takenRecentDamage = true;
        Debug.Log("Player immune from damage for " + recentDamageTimer + " seconds");
        yield return new WaitForSeconds(recentDamageTimer);
        takenRecentDamage = false;
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
}

