using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private UiManager uiManager;

    private Transform currentRespawnPoint;
    
    [Header("Stamina")]
    [SerializeField] private float staminaRegenPerTick; //per update tick

    [Header("Max Variables")]
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxAmmoReserve;
    [SerializeField] private float maxStamina;

    [Header("Start Variables")] //variables that are not max after respawn
    [SerializeField] private int startAmmoReserve;

    [Header("Pickup Amount")] //TODO consistent naming?
    [SerializeField] private int healthBoostAmount;
    [SerializeField] private int ammoBoxAmount;
    [SerializeField] private int staminaPickUpRestoreAmount;

    [Header("Timers")] //max timers are constant
    [SerializeField] private float timerUntilRespawnMax;
    [SerializeField] private float timerUntilStaminaComparisonCheckMax; //In frames (done in update)
    [SerializeField] private float timerUntilStaminaRegenMax; //In frames (done in update)
    [SerializeField] private float recentDamageTimerMax; //In frames (done in update)

    //Timers that changes during runtime
    private float timeUntilRespawn;
    private float timerUntilStaminaComparisonCheck; 
    private float timerUntilStaminaRegen;
    private float recentDamageTimer;

    //Used during runtime
    private int healthPoints;
    private int currentAmmoReserve;
    private float currentStamina;
    private bool takenRecentDamage = false;

    public static bool isAlive = true;


    public int GetCurrentHealthPoints() { return healthPoints; }
    public int GetCurrentAmmoReserve() { return currentAmmoReserve; }

    public float GetCurrentStamina() { return currentStamina; }
    public float GetMaxStamina() { return maxStamina; }
    public float GetMaxAmmoReserve() { return maxAmmoReserve; }
    public float GetMaxHealth() { return maxHealthPoints; }

    public bool IsPlayerAlive() { return isAlive; }

    private void Start()
    {
        healthPoints = maxHealthPoints;
        currentAmmoReserve = startAmmoReserve;
        currentStamina = maxStamina;

        currentRespawnPoint = spawnPoint.transform;

        ResetAllStats();

        timeUntilRespawn = timerUntilRespawnMax;
        timerUntilStaminaRegen = timerUntilStaminaRegenMax;
        timerUntilStaminaComparisonCheck = timerUntilStaminaComparisonCheckMax;
        recentDamageTimer = recentDamageTimerMax;
    }

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
            currentStamina -= amount;
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
        if (Input.GetKeyDown(KeyCode.L))
        {
            healthPoints = -1;
        }


        if (healthPoints <= 0)
        {
            isAlive = false;
            Debug.Log("PLAYER HAS DIED");
            uiManager.DeathText();

            PlayerMovement pm = gameObject.GetComponent<PlayerMovement>();

            timeUntilRespawn -= Time.deltaTime;

            if (timeUntilRespawn <= 0)
            {
                gameManager.GetComponent<EnemyRespawnHandler>().RepsawnAll();
                ResetAllStats();
                pm.TeleportPlayer(currentRespawnPoint.position);
                uiManager.DeathText();
                isAlive = true;
                timeUntilRespawn = timerUntilRespawnMax;
            }
        }
    }

    private void RecentDamageTaken() //to prevent massive amount of damage during a short period of time
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
        if (currentStamina < maxStamina && Time.timeScale == 1f) //stamina regen start, and checks if slow motion is NOT active
        {
            timerUntilStaminaComparisonCheck -= Time.deltaTime;

            if (timerUntilStaminaComparisonCheck <= 0) //may not need two timers here, might add some more checks here, therefore two timers
            {
                timerUntilStaminaRegen -= Time.deltaTime;

                if (timerUntilStaminaRegen <= 0)
                {
                    //Debug.Log("Stamina is now regenerating");
                    currentStamina += staminaRegenPerTick;
                    timerUntilStaminaComparisonCheck = timerUntilStaminaComparisonCheckMax;
                    timerUntilStaminaRegen = timerUntilStaminaRegenMax;
                }
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
            healthPoints = -1;
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