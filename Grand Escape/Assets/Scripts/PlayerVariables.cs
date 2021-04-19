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
    [SerializeField] int maxAmmoReserve;
    [SerializeField] float maxStamina;

    [Header("Start Variables")]
    [SerializeField] int startAmmoReserve;

    [Header("Pickup Amount")]
    [SerializeField] int healthBoostAmount;
    [SerializeField] int ammoBoxAmount;
    [SerializeField] int staminaPickUpRestoreAmount;

    [Header("Damage Variables")]
    [SerializeField] float recentDamageTimer;

    int healthPoints;
    int currentAmmoReserve;
    float currentStamina;
    bool takenRecentDamage = false;

    private void Awake()
    {
        Player = this.gameObject;
        healthPoints = maxHealthPoints;
        currentAmmoReserve = startAmmoReserve;
        currentStamina = maxStamina;

        currentRespawnPoint = respawnPoint.transform.position;
    }

    private void Start()
    {
        ResetAllStats();
    }

    public float GetCurrentStamina() { return currentStamina; }

    public int GetCurrentHealthPoints() { return healthPoints; }

    public int GetCurrentAmmoReserve() { return currentAmmoReserve; }

    public float GetMaxStamina() { return maxStamina; }

    public float GetMaxAmmoReserve() { return maxAmmoReserve; }

    public float GetMaxHealth() { return maxHealthPoints; }

    public void ReduceAmmoReserve(int amountToReduce)
    {
        if(currentAmmoReserve - amountToReduce < 0)
            currentAmmoReserve = 0;
        else
            currentAmmoReserve -= amountToReduce;
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
        currentAmmoReserve = startAmmoReserve;
        currentStamina = maxStamina;

        uiManager.HealthPoints(healthPoints);
        uiManager.Stamina((int)currentStamina);
        uiManager.AmmoStatus(currentAmmoReserve);
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
        if (other.gameObject.CompareTag("Ammo boost") && currentAmmoReserve < maxAmmoReserve)
        {
            currentAmmoReserve += ammoBoxAmount;
            Destroy(other.gameObject);

            if (currentAmmoReserve > maxAmmoReserve)
            {
                Debug.Log("Ammo restored to max");
                currentAmmoReserve = maxAmmoReserve;
            }
            else
                Debug.Log("Ammo restored by " + ammoBoxAmount);

            uiManager.AmmoStatus(GetCurrentAmmoReserve());
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

        if (healthPoints > 0)
        {
            uiManager.HealthPoints(healthPoints);
            uiManager.Stamina((int)currentStamina);
            uiManager.AmmoStatus(currentAmmoReserve);
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

