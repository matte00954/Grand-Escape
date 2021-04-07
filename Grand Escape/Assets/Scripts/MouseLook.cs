using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    
    public float mouseSensitivity = 100f; //Default mouse sensitivity value. Can be changed otherwise in Inspector.

    public Transform playerBody; //The player's model object.

    float xRotation = 0f; 

    // Start is called before the first frame update
    void Start()
    {
        //This locks the mouse cursor to the game screen and hides it.
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY; //Looking up or down means you're rotating your view along the X-axis.
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); //This makes it so that you can't look further past than straight down and up.
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX); //This rotates the player model along the Y-axis when moving the mouse left or right.
    }
}
