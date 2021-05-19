//Main author: Mattias Larsson
//Secondary author: William Örnquist
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private UiManager uiManager;

    [SerializeField] private AudioManager audioManager;

    private Transform currentRespawnPoint;
    
    [Header("Stamina")]
    [SerializeField] private float staminaRegenPerTick; //per update tick

    [Header("Max Variables")]
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxAmmoReserve;
    [SerializeField] private float maxStamina;

    [Header("Start Variables")] //variables that are not max after respawn
    [SerializeField] private int startAmmoReserve;

    //[Header("Pickup Amount")] //TODO consistent naming?
    //[SerializeField] private int healthBoostAmount;
    //[SerializeField] private int ammoBoxAmount;
    //[SerializeField] private int staminaPickUpRestoreAmount;

    [Header("Timers")] //max timers are constant
    [SerializeField] private float timerUntilRespawnMax;
    [SerializeField] private float timerUntilStaminaComparisonCheckMax; //In frames (done in update)
    [SerializeField] private float timerUntilStaminaRegenMax; //In frames (done in update)
    [SerializeField] private float recentDamageTimerMax; //In frames (done in update)


    [Header("Cheats for testing")] //for testing
    [SerializeField] private bool playerSuicideAvailable;
    [SerializeField] private bool godMode;
    [SerializeField] private bool unlockAllWeapons;


    private string playerDamageTakenSound = "PlayerDamageTaken";

    //Timers that changes during runtime
    private float timeUntilRespawn;
    private float timerUntilStaminaComparisonCheck; 
    private float timerUntilStaminaRegen;

    //Used during runtime
    private int healthPoints;
    private int currentAmmoReserve;
    private float currentStamina;

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
        if (godMode)
            maxHealthPoints = 9999;

        if (unlockAllWeapons)
        {
            for (int index = 0; index < 3; index++)
                WeaponHolder.UnlockWeaponSlot(index);
        }

        healthPoints = maxHealthPoints;

        currentAmmoReserve = startAmmoReserve;
        currentStamina = maxStamina;

        currentRespawnPoint = spawnPoint.transform;

        ResetAllStats();

        timeUntilRespawn = timerUntilRespawnMax;
        timerUntilStaminaRegen = timerUntilStaminaRegenMax;
        timerUntilStaminaComparisonCheck = timerUntilStaminaComparisonCheckMax;
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
        if (!PlayerMovement.IsDodging)
        {
            Debug.Log("Player took :" + damageToBeApplied + " damage");
            healthPoints -= damageToBeApplied;
            uiManager.TakenDamage();
            audioManager.Play(playerDamageTakenSound);
            uiManager.HealthPoints(healthPoints);
        }
        else
            Debug.Log("Player absorbed damage on Dodge");
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

    public void AddingStatAfterPickup(string statToChange, int amount)
    {
        if (statToChange.Contains("stamina"))
        {
            currentStamina += amount;
            Debug.Log("Stamina restored by " + amount);

            if (currentStamina > maxStamina)
            {
                currentStamina = maxStamina;
            }
        }

        if (statToChange.Contains("health"))
        {
            healthPoints += amount;
            Debug.Log("Health restored by " + amount);

            if (healthPoints > maxHealthPoints)
            {
                healthPoints = maxHealthPoints;
            }
        }

        if (statToChange.Contains("ammo"))
        {
            currentAmmoReserve += amount;

            Debug.Log("Ammo restored by " + amount);

            if (currentAmmoReserve > maxAmmoReserve)
            {
                currentAmmoReserve = maxAmmoReserve;
            }
        }
    }

    private void Update()
    {
        StaminaRegen();
        UiUpdate();
        PlayerDeath();

        if (playerSuicideAvailable)
            PlayerSuicide();
    }

    private void PlayerSuicide() //For testing
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            healthPoints = -1;
        }
    }

    private void PlayerDeath()
    {

        if (healthPoints <= 0)
        {
            isAlive = false;
            Debug.Log("PLAYER HAS DIED");
            uiManager.DeathText(true);
            FindObjectOfType<CamAnimation>().PlayDeathAnimation();

            PlayerMovement pm = gameObject.GetComponent<PlayerMovement>();

            timeUntilRespawn -= Time.deltaTime;

            if (timeUntilRespawn <= 0)
            {
                gameManager.GetComponent<CheckpointRespawnHandler>().RepsawnAll();
                ResetAllStats();
                pm.TeleportPlayer(currentRespawnPoint.position);
                uiManager.DeathText(false);
                isAlive = true;
                timeUntilRespawn = timerUntilRespawnMax;
            }
        }
    }

    //private void RecentDamageTaken() //to prevent massive amount of damage during a short period of time
    //{
    //    if (takenRecentDamage)
    //    {

    //        recentDamageTimer -= Time.deltaTime;

    //        if (recentDamageTimer <= 0)
    //        {
    //            uiManager.TakenDamage(false);
    //            Debug.Log("Player can take damage again");
    //            takenRecentDamage = false;
    //        }
    //    }
    //}

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
        //Pickups(other);
        CheckPointAndEndScene(other);
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

}