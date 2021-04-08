using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text ammoLeft;
    public Text weaponStatus;
    public Text currentHP;

    public void WeaponStatus(string s)
    {
        weaponStatus.text = s;
    }

    public void AmmoStatus(int i)
    {
        ammoLeft.text = i.ToString();
    }

    public void HealthPoints(int i)
    {
        currentHP.text = i.ToString();
    }
}
