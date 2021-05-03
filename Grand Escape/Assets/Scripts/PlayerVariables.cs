using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerVariables : MonoBehaviour
{

    [SerializeField] UiManager uiManager;

    [SerializeField] GameObject spawnPoint;

    [SerializeField] GameObject gameManager;

    Transform currentRespawnPoint;

    GameObject player;

    [Header("Stamina")]
    [SerializeField] float staminaRegenPerTick;

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

    [Header("Timers")]   
    [SerializeField] float timerUntilRespawnMax;
    [SerializeField] float timerUntilStaminaComparisonCheckMax; //In frames (done in update)
    [SerializeField] float timerUntilStaminaRegenMax; //In frames (done in update)
    [SerializeField] float recentDamageTimerMax; //In frames (done in update)

    float timeUntilRespawn;
    float timerUntilStaminaComparisonCheck; //In frames (done in update)
    float timerUntilStaminaRegen; //In frames (done in update)
    float recentDamageTimer;

    int healthPoints;
    int currentAmmoReserve;
    float currentStamina;
    bool takenRecentDamage = false;

    bool isDead = false;

    private void Awake()
    {
        player = this.gameObject;
        healthPoints = maxHealthPoints;
        currentAmmoReserve = startAmmoReserve;
        currentStamina = maxStamina;
    }

    private void Start()
    {
        currentRespawnPoint = spawnPoint.transform;
        ResetAllStats();
        //TODO SÄTT ALLA TIMERS HÄR


        timerUntilStaminaRegen = timerUntilStaminaRegenMax;
        timerUntilStaminaComparisonCheck = timerUntilStaminaComparisonCheckMax;

        recentDamageTimer = recentDamageTimerMax;
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
            takenRecentDamage = true;
        }
        uiManager.HealthPoints(healthPoints);
    }

    public void ResetAllStats() //should work on player death too
    {
        healthPoints = maxHealthPoints;
        currentAmmoReserve = startAmmoReserve;
        currentStamina = maxStamina;

        uiManager.HealthPoints(healthPoints);
        uiManager.Stamina((int)currentStamina);
        uiManager.AmmoStatus(currentAmmoReserve);
    }

    public void SetNewRespawnPoint(GameObject newRespawnPoint) 
    {
        Debug.Log("Old respawn point " + currentRespawnPoint.transform.position);
        spawnPoint.GetComponent<MoveRespawnPoint>().SetNewPoint(newRespawnPoint.transform.position);
        Debug.Log("New respawn point " + currentRespawnPoint.transform.position);
    }

    public void StaminaToBeUsed(float amount) //Everything that costs stamina should use this method
    {

        if (currentStamina >= 0)
        {
            currentStamina -= amount;
        }
        else 
            Debug.Log("Out of stamina");
    }

    private void Update()
    {
        PlayerDeath();

        StaminaRegen();

        UiUpdate();

        RecentDamageTaken();
    }

    private void PlayerDeath()
    {
        if (healthPoints <= 0)
        {
            isDead = true;

            PlayerMovement pm = GetComponent<PlayerMovement>();
            Debug.Log("PLAYER HAS DIED");

            timeUntilRespawn -= Time.deltaTime;

            pm.SetControllerInactive();

            if (timeUntilRespawn <= 0)
            {
                gameManager.GetComponent<EnemyRespawnHandler>().RepsawnAll();
                pm.TeleportPlayer(currentRespawnPoint.position);
                ResetAllStats();
                timeUntilRespawn = timerUntilRespawnMax;
            }
        }
    }

    public bool IsPlayerDead()
    {
        return isDead;
    }

    private void RecentDamageTaken()
    {
        if (takenRecentDamage)
        {
            recentDamageTimer -= Time.deltaTime;

            if (recentDamageTimer <= 0)
            {
                takenRecentDamage = false;
                Debug.Log("Player can take damage again");
                recentDamageTimer = recentDamageTimerMax;
            }
        }
    }

    private void UiUpdate()
    {
        uiManager.HealthPoints(healthPoints);
        uiManager.Stamina((int)currentStamina);
        uiManager.AmmoStatus(currentAmmoReserve);
    }

    private void StaminaRegen()
    {
        if (currentStamina < maxStamina && Time.timeScale == 1) //stamina regen start, and checks if slow motion is NOT active
        {

            timerUntilStaminaComparisonCheck -= Time.deltaTime;

            //float oldStamina = currentStamina;

            if (timerUntilStaminaComparisonCheck <= 0)
            {
                timerUntilStaminaRegen -= Time.deltaTime;

                if (timerUntilStaminaRegen <= 0)
                {
                    Debug.Log("Stamina is now regenerating");
                    currentStamina += staminaRegenPerTick;
                }
                //else
                //{
                //    Debug.Log("Stamina != old stamina");
                //    timerUntilStaminaRegen = timerUntilStaminaRegenMax;
                //    timerUntilStaminaComparisonCheck = timerUntilStaminaComparisonCheckMax;
                //}
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Pickups(other);

        CheckPointAndEndScene(other);

        PlayerDrown(other);
    }

    private void CheckPointAndEndScene(Collider other) //For checkpoints and ending scenes
    {
        if (other.gameObject.CompareTag("Check point"))
        {
            SetNewRespawnPoint(other.gameObject);
            other.gameObject.SetActive(false);
            //Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("Scene end"))
        {
            gameManager.GetComponent<SceneSwitch>().ChangeScene();
        }
    }

    private void PlayerDrown(Collider other) //Player should die from touching this collider trigger
    {
        if (other.gameObject.CompareTag("Water"))
        {
            healthPoints = 0;
        }
    }

    private void Pickups(Collider other) //All pickups should be here
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
        }
    }
}