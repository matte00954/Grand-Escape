//Main author: Mattias Larsson
//Secondary author: William Örnquist
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Text ammoLeftText;
    [SerializeField] private Text slowMotionExhaustionText;
    [SerializeField] private Text sprintExhaustionText;
    [SerializeField] private Text tutorialText;

    [SerializeField] private Slider healthPointSlider;
    [SerializeField] private Slider staminaPointSlider;
    [SerializeField] private Slider weaponReloadedSlider;
    [SerializeField] private Slider dodgeCooldownSlider;

    [SerializeField] private Image crouchImage;
    [SerializeField] private Image deathImage;
    [SerializeField] private Image recentDamageTakenImage;

    [SerializeField] private WeaponHolder weaponHolder;

    public void TutorialText(string textToShow, bool active)
    {
        if (active)
        {
            tutorialText.text = textToShow;
            tutorialText.gameObject.SetActive(true);
        }
        else
            tutorialText.gameObject.SetActive(false);
    }

    public void WeaponStatus(bool isReloaded)
    {
        if (weaponHolder.GetSelectedWeapon() == 2)
        {
            weaponReloadedSlider.value = 0f;
        }
        else
        {
            if (isReloaded)
                weaponReloadedSlider.value = 100f;
            else
                weaponReloadedSlider.value = 0f;

            Debug.Log("is reloaded = " + isReloaded);
        }
    }

    public void AmmoStatus(int i) => ammoLeftText.text = i.ToString();

    public void HealthPoints(int healthPoints) => healthPointSlider.value = healthPoints;

    public void Stamina(int staminaPoints) => staminaPointSlider.value = staminaPoints;

    public void DodgeCooldown(float cooldownTimer, float cooldownCapacity) //Value in inspector needs to be the same as value currently assigned in player movement
    {
        dodgeCooldownSlider.value = cooldownCapacity - cooldownTimer;
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

    public void CrouchingImage(bool active)
    {
        if (active)
            crouchImage.gameObject.SetActive(true);
        else
            crouchImage.gameObject.SetActive(false);
    }

    public void DeathText(bool isAlive) //when player dies
    {
        if (isAlive) //PlayerVariables.IsAlive does not work here
            deathImage.gameObject.SetActive(true);
        else
            deathImage.gameObject.SetActive(false);
    }

    public void TakenDamage(bool recentDamageTaken) //when recentDamageTaken is active
    {
        recentDamageTakenImage.gameObject.SetActive(recentDamageTaken);
    }
}