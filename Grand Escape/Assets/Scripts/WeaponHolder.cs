//Main author: William Örnquist
//Secondary author: Mattias Larsson
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    private static bool unlockedFlintPistol, unlockedMusket, unlockedSword;
    private static int selectedWeapon = -1;

    private void Update()
    {
        int previousSelectedWep = selectedWeapon;

        //if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        //{
        //    if(selectedWeapon >= transform.childCount - 1)
        //        selectedWeapon = 0;
        //    else
        //        selectedWeapon++;
        //}

        //if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        //{
        //    if (selectedWeapon >= 0)
        //        selectedWeapon = transform.childCount - 1;
        //    else
        //        selectedWeapon--;
        //}

        if (PlayerVariables.isAlive)
        {

            if (Input.GetKeyDown(KeyCode.Alpha1) && unlockedFlintPistol)
                selectedWeapon = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2) && unlockedMusket)
                selectedWeapon = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3) && unlockedSword)
                selectedWeapon = 2;

            if (previousSelectedWep != selectedWeapon)
                SelectWeapon();
        }
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

    public static void UnlockWeaponSlot(int index)
    {
        switch (index)
        {
            case 0:
                unlockedFlintPistol = true;
                break;
            case 1:
                unlockedMusket = true;
                break;
            case 2:
                unlockedSword = true;
                break;
            default:
                Debug.LogError("UnlockWeaponSlot: Invalid index");
                return;
        }
        selectedWeapon = index;
        Debug.Log("Unlocked weapon slot: " + index);
    }

    public int GetSelectedWeapon()
    {
        return selectedWeapon;
    }

}
