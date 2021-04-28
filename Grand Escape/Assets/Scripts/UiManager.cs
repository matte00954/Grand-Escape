using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Text ammoLeft;
    [SerializeField] Text weaponStatus;
    [SerializeField] Text currentHP;
    [SerializeField] Text currentStamina;
    [SerializeField] Text slowMoExhaustion;
    [SerializeField] Text sprintExhaustion;

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

    public void Stamina(int i)
    {
        currentStamina.text = i.ToString();
    }

    public void SlowMotionExhaustion(bool isExhaustedFromSlowMotion)
    {
        if (isExhaustedFromSlowMotion)
            slowMoExhaustion.gameObject.SetActive(true);
        else
            slowMoExhaustion.gameObject.SetActive(false);
    }

    public void SprintExhaustion(bool isExhaustedFromSprinting)
    {
        if (isExhaustedFromSprinting)
            sprintExhaustion.gameObject.SetActive(true);
        else
            sprintExhaustion.gameObject.SetActive(false);
    }
}