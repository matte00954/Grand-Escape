//Main author: Mattias Larsson
//Secondary author: William Örnquist
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    [SerializeField] private Text ammoLeftText;
    [SerializeField] private Text tutorialText;

    [SerializeField] private Slider healthPointSlider;
    [SerializeField] private Slider staminaPointSlider;
    [SerializeField] private Slider weaponReloadedSlider;
    [SerializeField] private Slider dodgeCooldownSlider;

    [SerializeField] private Image slowMotionExhaustion;
    [SerializeField] private Image sprintExhaustion;
    [SerializeField] private Image tutorial;

    [SerializeField] private Image crouchImage;
    [SerializeField] private Image deathImage;
    [SerializeField] private Image slowMotionImage;
    [SerializeField] private Image recentDamageTakenImage;

    [SerializeField] private Image weaponStatusBorder;

    [SerializeField] private float recentDamageTakenTimerMax;

    private float recentDamageTakenTimer;

    private bool recentDamage;

    public void TutorialText(string textToShow, bool active)
    {
        if (active)
        {
            tutorialText.text = textToShow;
            tutorial.gameObject.SetActive(true);
        }
        else
            tutorial.gameObject.SetActive(false);
    }

    public void WeaponStatus(int isReloaded) //0 == false, 1 == true, 2 == meele
    {
        if (isReloaded == 2)
        {
            weaponReloadedSlider.value = 0f;
        }
        else
        {
            weaponStatusBorder.gameObject.SetActive(true);
            if (isReloaded == 1)
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
            slowMotionExhaustion.gameObject.SetActive(true);
        else
            slowMotionExhaustion.gameObject.SetActive(false);
    }

    public void SprintExhaustion(bool isExhaustedFromSprinting)
    {
        if (isExhaustedFromSprinting)
            sprintExhaustion.gameObject.SetActive(true);
        else
            sprintExhaustion.gameObject.SetActive(false);
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

    public void TakenDamage() //when recentDamageTaken is active
    {
        recentDamageTakenImage.gameObject.SetActive(true);
        recentDamage = true;
    }

    public void SlowMotionEffect()
    {
        if (Time.timeScale == 1)
        {
            slowMotionImage.gameObject.SetActive(false);
        }
        else
            slowMotionImage.gameObject.SetActive(true);
    }

    private void Start()
    {
        recentDamageTakenTimer = recentDamageTakenTimerMax;
    }

    private void Update()
    {
        if (recentDamage)
        {
            recentDamageTakenTimer -= Time.deltaTime;
            if (recentDamageTakenTimer <= 0)
            {
                recentDamageTakenImage.gameObject.SetActive(false);
                recentDamage = false;
                recentDamageTakenTimer = recentDamageTakenTimerMax;
            }
        }
    }
}