using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] Text ammoLeftText;
    [SerializeField] Text weaponReloadedText; //Not used at the moment, should tell the player if weapon is reloaded or not
    [SerializeField] Text currentHealthPointsText;
    [SerializeField] Text currentStaminaText;
    [SerializeField] Text slowMotionExhaustionText;
    [SerializeField] Text sprintExhaustionText;
    [SerializeField] Text tutorialText;

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

    public void WeaponStatus(string s)
    {
        weaponReloadedText.text = s;
    }

    public void AmmoStatus(int i)
    {
        ammoLeftText.text = i.ToString();
    }

    public void HealthPoints(int i)
    {
        currentHealthPointsText.text = i.ToString();
    }

    public void Stamina(int i)
    {
        currentStaminaText.text = i.ToString();
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