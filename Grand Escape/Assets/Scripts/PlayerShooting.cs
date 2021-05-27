//Author: Mattias Larsson
//Author: William Örnquist
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab; //Assign prefab
    [SerializeField] private Weapons weaponType; //Assign weapon type from Weapons folder
    [SerializeField] private float slowMotionReloadSpeedDivider = 2;
    [SerializeField] private ParticleSystem gunSmoke; //Assign prefab

    [SerializeField] private float timeFireSoundMax;
    private float timerFireSound;

    private UiManager uiManager;

    private Camera playerCamera;
    private PlayerVariables playerVariables;
    private CharacterController charController;
    private Animator animator;
    private AudioManager audioManager;

    private bool isReloading;
    private bool justFired;
    private int currentAmmoLoaded; //shots that are loaded

    private float reloadTimer;

    private void Awake() => uiManager = FindObjectOfType<UiManager>();

    private void Start()
    {
        playerCamera = GetComponentInParent<Camera>();
        playerVariables = GetComponentInParent<PlayerVariables>();
        charController = GetComponentInParent<CharacterController>();
        animator = GetComponent<Animator>();
        audioManager = FindObjectOfType<AudioManager>();

        isReloading = false;
        currentAmmoLoaded = weaponType.GetAmmoCap();

        if (currentAmmoLoaded == 1)
            uiManager.WeaponStatus(1);
        else
            uiManager.WeaponStatus(0);
    }

    private void OnEnable()
    {
        if(currentAmmoLoaded == 1)
            uiManager.WeaponStatus(1);
        else
            uiManager.WeaponStatus(0);
    }

    private void OnDisable()
    {
        if (isReloading)
        {
            Debug.Log("Canceling reload");
            reloadTimer = weaponType.GetReloadTime();
            isReloading = false;
        }
    }

    private void Update()
    {
        if (PlayerVariables.isAlive)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            animator.SetBool("Moving", inputX != 0 && charController.isGrounded || inputZ != 0 && charController.isGrounded);
            animator.SetFloat("TimeScale", Time.timeScale);

            Vector3 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) && currentAmmoLoaded > 0)
            {
                currentAmmoLoaded--;
                audioManager.Play(weaponType.GetSoundWeaponClick());
                animator.SetTrigger("Fire");
                uiManager.WeaponStatus(0);
                Instantiate(bulletPrefab, point, playerCamera.transform.rotation);

                Instantiate(gunSmoke, point, playerCamera.transform.rotation);

                timerFireSound = timeFireSoundMax;

                justFired = true;
            }
            else if(Input.GetMouseButtonDown(0) && currentAmmoLoaded <= 0)
            {
                Debug.Log("Weapon empty");
                audioManager.Play(weaponType.GetSoundWeaponClick());
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            {
                if (currentAmmoLoaded < weaponType.GetAmmoCap() && playerVariables.GetCurrentAmmoReserve() > 0)
                {
                    isReloading = true;
                    
                    animator.SetTrigger("Reload");
                }
                else if (playerVariables.GetCurrentAmmoReserve() == 0)
                    Debug.Log("No ammo left");
                else if (playerVariables.GetCurrentAmmoReserve() < 0)
                    Debug.LogError("ERROR: CURRENT AMMO IS LOWER THAN ZERO");
            }

            if (isReloading)
                UpdateReload();
        }

        if (justFired)
        {
            timerFireSound -= Time.deltaTime;
            if (timerFireSound > 0)
            {
                audioManager.Play(weaponType.GetSoundFire());
                justFired = false;
            }
        }
    }

    private void UpdateReload()
    {
        float reloadTime = weaponType.GetReloadTime();

        if (Time.timeScale < 1)
            reloadTime /= slowMotionReloadSpeedDivider;

        if (reloadTimer > 0f)
            reloadTimer -= Time.deltaTime;
        else if (reloadTimer <= 0f)
        {
            //audioManager.Play(weaponType.GetSoundReloadFinish()); //redundant for new animation
            //animator.SetTrigger("FinishReload"); //redundant for new animation

            reloadTimer = reloadTime;
            if (playerVariables.GetCurrentAmmoReserve() < weaponType.GetAmmoCap())
                currentAmmoLoaded = playerVariables.GetCurrentAmmoReserve();
            else
                currentAmmoLoaded = weaponType.GetAmmoCap();

            uiManager.WeaponStatus(1);
            playerVariables.ReduceAmmoReserve(weaponType.GetAmmoCap());
            isReloading = false;
        }
    }

    public void PlayReloadSound() => audioManager.Play(weaponType.GetSoundReloadStart());
}
