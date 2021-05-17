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

        isReloading = false;
        currentAmmoLoaded = weaponType.GetAmmoCap();

        if (currentAmmoLoaded == 1)
            uiManager.WeaponStatus(true);
        else
            uiManager.WeaponStatus(false);
    }

    private void OnEnable()
    {
        if(currentAmmoLoaded == 1)
            uiManager.WeaponStatus(true);
        else
            uiManager.WeaponStatus(false);
    }

    private void OnDisable()
    {
        if (isReloading)
        {
            Debug.Log("Canceling reload");
            reloadTimer = 0f;
            isReloading = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerVariables.isAlive)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            animator.SetBool("Moving", (inputX != 0 && charController.isGrounded || inputZ != 0 && charController.isGrounded));

            Vector3 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);

            //playerAim = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) && currentAmmoLoaded > 0)
            {
                currentAmmoLoaded--;
                FindObjectOfType<AudioManager>().Play(weaponType.GetSoundWeaponClick());
                animator.SetTrigger("Fire");
                uiManager.WeaponStatus(false);
                Instantiate(bulletPrefab, point, playerCamera.transform.rotation);

                Instantiate(gunSmoke, point, playerCamera.transform.rotation);

                timerFireSound = timeFireSoundMax;

                justFired = true;
            }
            else if(Input.GetMouseButtonDown(0) && currentAmmoLoaded <= 0)
            {
                Debug.Log("Weapon empty");
                FindObjectOfType<AudioManager>().Play(weaponType.GetSoundWeaponClick());
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            {
                if (currentAmmoLoaded < weaponType.GetAmmoCap() && playerVariables.GetCurrentAmmoReserve() > 0)
                {
                    isReloading = true;
                    FindObjectOfType<AudioManager>().Play(weaponType.GetSoundReloadStart());
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
                FindObjectOfType<AudioManager>().Play(weaponType.GetSoundFire());
                justFired = false;
            }
        }
    }

    private void UpdateReload()
    {
        float reloadTime = weaponType.GetReloadTime();

        if (Time.timeScale < 1)
            reloadTime /= slowMotionReloadSpeedDivider;

        if (reloadTimer < reloadTime)
            reloadTimer += Time.deltaTime;
        else if (reloadTimer >= reloadTime)
        {
            FindObjectOfType<AudioManager>().Play(weaponType.GetSoundReloadFinish());
            animator.SetTrigger("FinishReload");

            reloadTimer = 0f;
            if (playerVariables.GetCurrentAmmoReserve() < weaponType.GetAmmoCap())
                currentAmmoLoaded = playerVariables.GetCurrentAmmoReserve();
            else
                currentAmmoLoaded = weaponType.GetAmmoCap();

            uiManager.WeaponStatus(true);
            playerVariables.ReduceAmmoReserve(weaponType.GetAmmoCap());
            isReloading = false;
        }
    }
}
