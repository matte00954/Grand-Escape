using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    WeaponHolder weaponHolder;

    [SerializeField] Transform playerBody; //The player's model object.
    [SerializeField] Camera camera;

    [SerializeField] float mouseSensitivity = 100f; //Default mouse sensitivity value. Can be changed otherwise in Inspector.
    [SerializeField] float fovForZoom = 20;

    float xRotation = 0f;
    float defaultFov;

    bool isZoomed;

    bool canZoom;

    // Start is called before the first frame update
    void Start()
    {
        //This locks the mouse cursor to the game screen and hides it.
        Cursor.lockState = CursorLockMode.Locked;
        defaultFov = camera.fieldOfView;

        weaponHolder = FindObjectOfType<WeaponHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale != 0)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isZoomed && weaponHolder.GetSelectedWeapon() == 1) //1 is musket
            {
                camera.fieldOfView = fovForZoom;
                isZoomed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && isZoomed)
            {
                camera.fieldOfView = defaultFov;
                isZoomed = false;
            }

            if (weaponHolder.GetSelectedWeapon() != 1) //1 is musket
            {
                camera.fieldOfView = defaultFov;
            }


            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;

            xRotation -= mouseY; //Looking up or down means you're rotating your view along the X-axis.
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); //This makes it so that you can't look further past than straight down and up.
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX); //This rotates the player model along the Y-axis when moving the mouse left or right.
        }
    }
}
