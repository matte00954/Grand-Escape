//Main author: Mattias Larsson
//Secondary author: William Örnquist
using UnityEngine;

public class PlayerVariables : MonoBehaviour
{
    //[SerializeField] private GameObject spawnPoint;
    [SerializeField] private GameObject gameManager;
    [SerializeField] private UiManager uiManager;

    [SerializeField] private AudioManager audioManager;

    [Header("Stamina")]
    [SerializeField] private float staminaRegenerationPerTick; //per update tick
    [Tooltip("The amount of stamina regenerated from each kill during slow motion."),
        SerializeField] private float staminaLeechAmount;

    [Header("Max Variables")]
    [SerializeField] private int maxHealthPoints;
    [SerializeField] private int maxAmmoReserve;
    [SerializeField] private float maxStamina;

    [Header("Start Variables")] //variables that are not max after respawn
    [SerializeField] private int startAmmoReserve;

    [Header("Timers")] //max timers are constant
    [SerializeField] private float timerUntilRespawnMax;
    [SerializeField] private float timerUntilStaminaComparisonCheckMax; //In frames (done in update)
    [SerializeField] private float timerUntilStaminaRegenMax; //In frames (done in update)
    [SerializeField] private float recentDamageTimerMax; //In frames (done in update)


    [Header("Cheats for testing")] //for testing
    [SerializeField] private bool playerSuicideAvailable;
    [SerializeField] private bool godMode;
    [SerializeField] private bool unlockAllWeapons;


    private string playerDamageTakenSound = "PlayerDamageTakenSound";
    private string playerDeathSound = "PlayerDeathSound";

    //Timers that changes during runtime
    private float timerUntilRespawn;
    private float timerUntilStaminaComparisonCheck;
    private float timerUntilStaminaRegen;

    //Used during runtime
    private int healthPoints;
    private int ammoReserve;

    private int checkPoint; 

    private static float maximumStamina;
    private static float stamina;

    /// <summary>
    /// The current stamina of the player in realtime. Setting a value will add up to total.
    /// </summary>
    public static void AddStamina(float amountToAdd, bool isLeech)
    {
        if (isLeech && Time.timeScale < 1f || !isLeech)
        {
            stamina += amountToAdd;
            if (stamina > maximumStamina)
            {
                stamina = maximumStamina;
            }
        }
    }

    public static bool isAlive = true;

    public int GetCurrentAmmoReserve() { return ammoReserve; }
    public int GetCurrentHealthPoints() { return healthPoints; }
    public int GetCurrentCheckPoint() { return checkPoint; }
    public float GetCurrentStamina() { return stamina; }


    public void SetCheckpointIndex(int index) => checkPoint = index; 

    private void Start()
    {
        maximumStamina = maxStamina;

        if (godMode)
            maxHealthPoints = 9999;

        if (unlockAllWeapons)
        {
            for (int index = 0; index < 3; index++)
                WeaponHolder.UnlockWeaponSlot(index);
        }

        healthPoints = maxHealthPoints;

        ammoReserve = startAmmoReserve;
        stamina = maxStamina;

        //checkPoint = 0;

        ResetAllStats();

        LoadPlayerStats();

        timerUntilRespawn = timerUntilRespawnMax;
        timerUntilStaminaRegen = timerUntilStaminaRegenMax;
        timerUntilStaminaComparisonCheck = timerUntilStaminaComparisonCheckMax;
    }

    public void SetStatsAfterSaveLoad(int savedHealthPoints, int savedAmmoReserve, float savedStamina, int savedCheckPoint)
    {
        healthPoints = savedHealthPoints;
        ammoReserve = savedAmmoReserve;
        stamina = savedStamina;
        checkPoint = savedCheckPoint;
    }

    public void ReduceAmmoReserve(int amountToReduce) //After reload
    {
        if(ammoReserve - amountToReduce < 0)
            ammoReserve = 0;
        else
            ammoReserve -= amountToReduce;
    }

    public void ApplyDamage(int damageToBeApplied)
    {
        if (!PlayerMovement.IsDodging)
        {
            Debug.Log("Player took :" + damageToBeApplied + " damage");
            healthPoints -= damageToBeApplied;
            uiManager.TakenDamage();
            GetComponent<LowVariablesAudio>().PlayDamageSound();
            uiManager.HealthPoints(healthPoints);
        }
        else
            Debug.Log("Player absorbed damage on Dodge");
    }

    public void ResetAllStats() //should work on player death too
    {
        healthPoints = maxHealthPoints;
        ammoReserve = startAmmoReserve;
        stamina = maxStamina;

        uiManager.HealthPoints(healthPoints);
        uiManager.Stamina((int)stamina);
        uiManager.AmmoStatus(ammoReserve);
    }


    public void StaminaToBeUsed(float amount) //Everything that costs stamina should use this method
    {
        if (!godMode)
        {
            if (stamina >= 0)
                stamina -= amount;
            else
                Debug.Log("Out of stamina");
        }
    }

    public void AddStatAfterPickup(string statToChange, int amount)
    {
        switch (statToChange) //remember to match string parameter to exactly these strings
        {
            case "stamina":
                stamina += amount;
                if (stamina > maxStamina)
                    stamina = maxStamina;
                Debug.Log("Stamina restored by " + amount);
                break;

            case "health":
                healthPoints += amount;
                if (healthPoints > maxHealthPoints)
                    healthPoints = maxHealthPoints;
                Debug.Log("Health restored by " + amount);
                break;

            case "ammo":
                ammoReserve += amount;
                if (ammoReserve > maxAmmoReserve)
                    ammoReserve = maxAmmoReserve;
                Debug.Log("Ammo restored by " + amount);
                break;

            case "pistol":
                WeaponHolder.UnlockWeaponSlot(0);
                Debug.Log("Unlocking pistol");
                break;

            case "musket":
                WeaponHolder.UnlockWeaponSlot(1);
                Debug.Log("Unlocking musket");
                break;

            case "sword":
                WeaponHolder.UnlockWeaponSlot(2);
                Debug.Log("Unlocking sword");
                break;

            default:
                break;
        }
    }

    private void Update()
    {
        StaminaRegeneration();
        UiUpdate();
        PlayerDeath();
        UpdateDeathTimer();

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

        if (healthPoints <= 0 && isAlive)
        {
            Debug.Log("PLAYER HAS DIED");

            uiManager.DeathText(true);

            //audioManager.Play(playerDeathSound);

            FindObjectOfType<CamAnimation>().PlayDeathAnimation();

            isAlive = false;
        }
    }

    private void UpdateDeathTimer()
    {
        if (!isAlive)
        {
            timerUntilRespawn -= Time.unscaledDeltaTime;

            if (timerUntilRespawn <= 0)
            {
                PlayerMovement pm = gameObject.GetComponent<PlayerMovement>();
                gameManager.GetComponent<CheckpointRespawnHandler>().RepsawnAll();
                ResetAllStats();
                pm.TeleportPlayer(gameManager.GetComponent<CheckpointRespawnHandler>().GetRespawnPoint());
                uiManager.DeathText(false);
                EnemyMovement.EaseAllEnemies(2f);
                isAlive = true;
                timerUntilRespawn = timerUntilRespawnMax;
            }
        }
    }
    private void UiUpdate()
    {
        uiManager.HealthPoints(healthPoints);
        uiManager.Stamina((int)stamina);
        uiManager.AmmoStatus(ammoReserve);
    }

    private void StaminaRegeneration()
    {
        if (stamina < maxStamina && Time.timeScale == 1f) //stamina regen start, and checks if slow motion is NOT active
        {
            timerUntilStaminaComparisonCheck -= Time.deltaTime;

            if (timerUntilStaminaComparisonCheck <= 0) //This might seem a bit weird, i had another plan for this originally, but this works fairly well and i do not have time for a better solution
            {
                timerUntilStaminaRegen -= Time.deltaTime;

                if (timerUntilStaminaRegen <= 0) //timer until actual regeneration
                {
                    //Debug.Log("Stamina is now regenerating");
                    stamina += staminaRegenerationPerTick;
                    timerUntilStaminaComparisonCheck = timerUntilStaminaComparisonCheckMax;
                    timerUntilStaminaRegen = timerUntilStaminaRegenMax;
                }
            }
        }
    }

    private void LoadPlayerStats()
    {
        if (LoadHandler.isSavedGame)
        {
            SaveAndLoadData saveAndLoadData = FindObjectOfType<SaveAndLoadData>();
            saveAndLoadData.Load(true);
        }
        else if (LoadHandler.sceneChanged)
        {
            SaveAndLoadData saveAndLoadData = FindObjectOfType<SaveAndLoadData>();
            saveAndLoadData.Load(false);
        }
        Debug.Log("SceneChanged is " + LoadHandler.sceneChanged);
        Debug.Log("isSavedGame is " + LoadHandler.isSavedGame);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Check point"))
        {
            other.gameObject.SetActive(false); //Ugly solution but this is the easiest solution to disable game objects, since checkpoints can not disable themselves.
        }
    }

    
}