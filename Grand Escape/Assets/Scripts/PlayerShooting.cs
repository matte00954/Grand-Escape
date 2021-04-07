using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{

    public Camera playerCamera;

    RaycastHit hit;

    Ray ray;

    // Start is called before the first frame update
    void Start()
    {
        hit;
        ray = playerCamera.ScreenPointToRay(Input.mousePosition);
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;
        }
    }
}
