//Main author: William Örnquist
//Secondary author: Mattias Larsson
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private Transform playerBody; //The player's model object transform.
    private Camera cam;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 100f; //Default mouse sensitivity value. Can be changed otherwise in Inspector.
    [SerializeField] private float fovForZoom = 20;

    private float xRotation;
    private float defaultFov;

    private bool isZoomed;
    private bool canZoom;

    private void Start()
    {
        cam = GetComponent<Camera>();

        //This locks the mouse cursor to the game screen and hides it.
        Cursor.lockState = CursorLockMode.Locked;
        defaultFov = cam.fieldOfView;
    }

    // Update is called once per frame
    private void Update()
    {
        if (PlayerVariables.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isZoomed && weaponHolder.GetSelectedWeapon() == 1) //1 is musket
            {
                cam.fieldOfView = fovForZoom;
                isZoomed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && isZoomed)
            {
                cam.fieldOfView = defaultFov;
                isZoomed = false;
            }

            if (weaponHolder.GetSelectedWeapon() != 1) //1 is musket
                cam.fieldOfView = defaultFov;

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.unscaledDeltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.unscaledDeltaTime;

            xRotation -= mouseY; //Looking up or down means you're rotating your view along the X-axis.
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); //This makes it so that you can't look further past than straight down and up.
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX); //This rotates the player model along the Y-axis when moving the mouse left or right.
        }
    }
}
