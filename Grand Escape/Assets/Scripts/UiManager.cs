using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Text ammoLeftText;
    [SerializeField] Text slowMotionExhaustionText;
    [SerializeField] Text sprintExhaustionText;
    [SerializeField] Text tutorialText;

    [SerializeField] Slider healthPointSlider;
    [SerializeField] Slider staminaPointSlider;
    [SerializeField] Slider weaponReloadedSlider;

    public void TutorialText(string s, bool active)
    {
        if (active)
        {
            tutorialText.text = s;
            tutorialText.gameObject.SetActive(true);
        }
        else
            tutorialText.gameObject.SetActive(false);
    }

    public void WeaponStatus(bool isReloaded)
    {
        if (isReloaded)
        {
            weaponReloadedSlider.value = 100;
        }
        else
            weaponReloadedSlider.value = 0;


        Debug.Log("is reloaded = " + isReloaded);
        Debug.Log("weapon slider value " + weaponReloadedSlider.value);
    }

    public void AmmoStatus(int i)
    {
        ammoLeftText.text = i.ToString();
    }

    public void HealthPoints(int healthPoints)
    {
        healthPointSlider.value = healthPoints;
    }

    public void Stamina(int staminaPoints)
    {
        staminaPointSlider.value = staminaPoints;
    }

    public void SlowMotionExhaustion(bool isExhaustedFromSlowMotion)
    {
        if (isExhaustedFromSlowMotion)
            slowMotionExhaustionText.gameObject.SetActive(true);
        else
            slowMotionExhaustionText.gameObject.SetActive(false);
    }

    public void SprintExhaustion(bool isExhaustedFromSprinting)
    {
        if (isExhaustedFromSprinting)
            sprintExhaustionText.gameObject.SetActive(true);
        else
            sprintExhaustionText.gameObject.SetActive(false);
    }
}