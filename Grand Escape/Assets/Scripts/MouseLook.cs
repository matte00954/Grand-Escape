//Main author: William Örnquist
//Secondary author: Mattias Larsson
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private WeaponHolder weaponHolder; //The script that allows weapon cycling.
    [SerializeField] private Transform playerBody; //The player's model object transform.
    private Camera cam;

    [Header("Settings")]
    [SerializeField] private float mouseSensitivity = 100f; //Default mouse sensitivity value. Can be changed otherwise in Inspector.
    [SerializeField] private float fovForZoom = 20;

    private float xRotation;
    private float defaultFov;

    private bool isZoomed;
    private bool canZoom;

    private readonly string mouseXName = "Mouse X";
    private readonly string mouseYName = "Mouse Y";

    private void Start()
    {
        cam = GetComponent<Camera>();

        //This locks the mouse cursor to the game screen and hides it.
        Cursor.lockState = CursorLockMode.Locked;
        defaultFov = cam.fieldOfView;
    }

    private void Update()
    {
        if (PlayerVariables.isAlive)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isZoomed && weaponHolder.GetSelectedWeapon() == 1) //Zooming is reserved for the musket rifle on array slot 1.
            {
                cam.fieldOfView = fovForZoom;
                isZoomed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Mouse1) && isZoomed)
            {
                cam.fieldOfView = defaultFov;
                isZoomed = false;
            }

            if (weaponHolder.GetSelectedWeapon() != 1) //1 is musket rifle
                cam.fieldOfView = defaultFov;

            float mouseX = Input.GetAxis(mouseXName) * mouseSensitivity * Time.unscaledDeltaTime; //unscaledDeltaTime retains the mouse sensitivity during slow motions.
            float mouseY = Input.GetAxis(mouseYName) * mouseSensitivity * Time.unscaledDeltaTime;

            xRotation -= mouseY; //Looking up or down means you're rotating your view along the X-axis.
            xRotation = Mathf.Clamp(xRotation, -90f, 90f); //This makes it so that you can't look further past than straight down and up.
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            playerBody.Rotate(Vector3.up * mouseX); //This rotates the player model along the Y-axis when moving the mouse left or right.
        }
    }
}
