using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab; //Assign prefab

    AudioManager audioManager;
    Camera playerCamera;
    PlayerVariables playerVariables;
    CharacterController charController;

    Animator animator;

    [Header("Weapon Stats")]
    [SerializeField] float reloadTime;

    [Header("Weapon Sounds")]
    [SerializeField] string audioFireName;
    [SerializeField] string audioStartReloadName;
    [SerializeField] string audioFinishReloadName;

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

        audioManager = FindObjectOfType<AudioManager>();

        animator = GetComponent<Animator>();

        isReloading = false;
        currentAmmoLoaded = clipCapacity;
    }

    private void OnEnable()
    {
        
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
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        animator.SetBool("Moving", (inputX != 0 && charController.isGrounded || inputZ != 0 && charController.isGrounded));

        Vector3 point = playerCamera.ScreenToWorldPoint(Input.mousePosition);

        //playerAim = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0) && currentAmmoLoaded > 0)
        {
            currentAmmoLoaded--;
            Instantiate(bulletPrefab, point, playerCamera.transform.rotation);

            animator.SetTrigger("Fire");
            audioManager.Play(audioFireName);

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
                //animator.SetTrigger("Reload");
                //audioManager.Play(audioStartReloadName);
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

    private void UpdateReload()
    {
        if (reloadTimer < reloadTime)
            reloadTimer += Time.deltaTime;
        else if (reloadTimer >= reloadTime)
        {
            animator.SetTrigger("FinishReload");
            audioManager.Play(audioFinishReloadName);

            reloadTimer = 0f;
            if (playerVariables.GetCurrentAmmoReserve() < clipCapacity)
                currentAmmoLoaded = playerVariables.GetCurrentAmmoReserve();
            else
                currentAmmoLoaded = clipCapacity;
            
            playerVariables.ReduceAmmoReserve(clipCapacity);
            
            isReloading = false;
        }
    }
}
