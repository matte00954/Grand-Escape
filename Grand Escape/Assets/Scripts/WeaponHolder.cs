using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    Animator animator;
    int selectedWeapon = 0;

    private void Start()
    {
        SelectWeapon();
    }

    private void Update()
    {
        int previousSelectedWep = selectedWeapon;

        if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if(selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon >= 0)
                selectedWeapon = transform.childCount - 1;
            else
                selectedWeapon--;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            selectedWeapon = 0;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedWeapon = 1;
        }

        if (previousSelectedWep != selectedWeapon)
            SelectWeapon();
    }

    private void SelectWeapon()
    {
        int index = 0;
        foreach (Transform weaponTransform in transform)
        {
            if (index == selectedWeapon)
                weaponTransform.gameObject.SetActive(true);
            else
            {
                weaponTransform.gameObject.GetComponent<Animator>().CrossFade("Idle", 0f);
                weaponTransform.gameObject.GetComponent<Animator>().Update(0f);
                weaponTransform.gameObject.SetActive(false);
            }

            index++;
        }
    }
}
