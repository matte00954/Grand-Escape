using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab; //Assign prefab
    [SerializeField] Weapons weaponType; //Assign weapon type from Weapons folder

    [SerializeField] float slowMotionReloadSpeedDivider = 2;

    [SerializeField] ParticleSystem gunSmoke; //Assign prefab

    UiManager uiManager;

    Camera playerCamera;
    PlayerVariables playerVariables;
    CharacterController charController;
    Animator animator;

    //bool isReloaded;

    [Header("Event System")]
    [SerializeField] UnityEvent OnReloadStart;
    [SerializeField] UnityEvent OnReloadFinish;
    [SerializeField] UnityEvent OnFire;



    private bool isReloading;
    private int currentAmmoLoaded; //shots that are loaded

    float reloadTimer;
    int clipCapacity = 1; //bullet clip capacity, 1 by default.

    private void Start()
    {
        playerCamera = GetComponentInParent<Camera>();

        playerVariables = GetComponentInParent<PlayerVariables>();
        charController = GetComponentInParent<CharacterController>();

        animator = GetComponent<Animator>();

        isReloading = false;
        currentAmmoLoaded = clipCapacity;

        uiManager = FindObjectOfType<UiManager>();
    }

    private void OnEnable()
    {
        if(currentAmmoLoaded > 0)
        {
            uiManager.WeaponStatus(true);
        }
        else
            uiManager.WeaponStatus(false);
    }

    private void OnDisable()
    {

        Debug.Log("OnDisable called");
        if (isReloading)
        {
            Debug.Log("Canceling reload");
            reloadTimer = 0f;
            isReloading = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            animator.SetBool("Moving", (inputX != 0 && charController.isGrounded || inputZ != 0 && charController.isGrounded));

            Vector3 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);

            //playerAim = playerCamera.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0) && currentAmmoLoaded > 0)
            {
                currentAmmoLoaded--;
                uiManager.WeaponStatus(false);
                Instantiate(bulletPrefab, point, playerCamera.transform.rotation);

                Instantiate(gunSmoke, point, playerCamera.transform.rotation);

                FindObjectOfType<AudioManager>().Play(weaponType.GetSoundFire());
                OnFire.Invoke();

                /*if (Physics.Raycast(playerAim, out shootHit)) //this raycast shooting, will probably not be used
                {
                    Transform objectHit = shootHit.transform;

                    //Debug.Log("Hit Object: " + objectHit);
                }*/
                //Debug.DrawRay(point, direction, Color.red); //denna funkar ej
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading)
            {
                if (currentAmmoLoaded < clipCapacity && playerVariables.GetCurrentAmmoReserve() > 0)
                {
                    isReloading = true;
                    FindObjectOfType<AudioManager>().Play(weaponType.GetSoundReloadStart());
                    OnReloadStart.Invoke();
                }
                else if (playerVariables.GetCurrentAmmoReserve() == 0)
                    Debug.Log("No ammo left");
                else if (playerVariables.GetCurrentAmmoReserve() < 0)
                    Debug.LogError("ERROR: CURRENT AMMO IS LOWER THAN ZERO");
            }

            if (isReloading)
                UpdateReload();
        }
    }

    private void UpdateReload()
    {
        float reloadTime = weaponType.GetReloadTime();

        if (Time.timeScale < 1)
        {
            reloadTime /= slowMotionReloadSpeedDivider;
        }

        if (reloadTimer < reloadTime)
            reloadTimer += Time.deltaTime;
        else if (reloadTimer >= reloadTime)
        {
            FindObjectOfType<AudioManager>().Play(weaponType.GetSoundReloadFinish());
            OnReloadFinish.Invoke();

            reloadTimer = 0f;
            if (playerVariables.GetCurrentAmmoReserve() < clipCapacity)
                currentAmmoLoaded = playerVariables.GetCurrentAmmoReserve();
            else
                currentAmmoLoaded = clipCapacity;

            uiManager.WeaponStatus(true);

            playerVariables.ReduceAmmoReserve(clipCapacity);

            isReloading = false;
        }
    }
}
