using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Text ammoLeftText;
    [SerializeField] Text currentHealthPointsText;
    [SerializeField] Text currentStaminaText;
    [SerializeField] Text slowMotionExhaustionText;
    [SerializeField] Text sprintExhaustionText;
    [SerializeField] Text tutorialText;

    [SerializeField] Image weaponReloadedImage;
    [SerializeField] Image weaponEmptyImage;

    [SerializeField] Slider healthPointSlider;
    [SerializeField] Slider staminaPointSlider;

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
        weaponReloadedImage.gameObject.SetActive(isReloaded);

        weaponEmptyImage.gameObject.SetActive(!isReloaded);

        //if (isReloaded)
        //{
        //    weaponReloadedImage.gameObject.SetActive(true);
        //}
        //else
        //    weaponReloadedImage.gameObject.SetActive(false);
    }

    public void AmmoStatus(int i)
    {
        ammoLeftText.text = i.ToString();
    }

    public void HealthPoints(int healthPoints)
    {
        healthPointSlider.value = healthPoints;
        //currentHealthPointsText.text = i.ToString();
    }

    public void Stamina(int staminaPoints)
    {
        staminaPointSlider.value = staminaPoints;
        //currentStaminaText.text = i.ToString();
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