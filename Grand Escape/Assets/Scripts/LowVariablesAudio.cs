using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author Leo Mendonca Agild leme2980
public class LowVariablesAudio : MonoBehaviour
{

    [SerializeField] private string[] stamminaSounds;
    [SerializeField] private string[] ammoSounds;
    [SerializeField] private string[] deathSounds;
    [SerializeField] private string[] hurtSounds;

    [SerializeField] private float stamminaCall;
    [SerializeField] private float ammoCall;
    [SerializeField] private float healthCall;

    private PlayerVariables variables;

    private bool refilledStammina;
    private bool saidPabst;
    private bool refilledAmmo;
    private bool refilledHealth;
    private bool died;

    // Start is called before the first frame update
    void Start()
    {
        variables = GetComponent<PlayerVariables>();
        refilledStammina = true;
        refilledAmmo = true;
        refilledHealth = true;
        saidPabst = false;
        died = false;
}

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(stamminaSounds.Length);

        //Debug.Log(Random.Range(0, soundNames.Length));
        if (variables.GetCurrentStamina() <= stamminaCall&&!saidPabst)
        {
            FindObjectOfType<AudioManager>().Play("NeedPabst");
            saidPabst = true;
        }
        if (!refilledStammina&& variables.GetCurrentStamina() >= 50)
        {
            refilledStammina = true;
            saidPabst = false;
        }
        if (variables.GetCurrentStamina()<=0 && refilledStammina)
        {
            //Debug.Log("tired");
            //Debug.Log(stamminaSounds[Random.Range(0, stamminaSounds.Length)]);
            FindObjectOfType<AudioManager>().Play(stamminaSounds[Random.Range(0, stamminaSounds.Length+1)]);
            refilledStammina = true;
        }


        if (variables.GetCurrentAmmoReserve() <= ammoCall && refilledAmmo)
        {
            FindObjectOfType<AudioManager>().Play(ammoSounds[Random.Range(0, ammoSounds.Length + 1)]);
            refilledAmmo = false;
        }
        if (!refilledAmmo && variables.GetCurrentAmmoReserve() >= 2)
        {
            refilledAmmo = true;
        }


        if (variables.GetCurrentHealthPoints() <= healthCall && refilledHealth)
        {
            FindObjectOfType<AudioManager>().Play("NeedHealth");
            refilledHealth = false;
        }
        if (!refilledHealth && variables.GetCurrentHealthPoints() >= 50)
        {
            refilledHealth = true;
            died = false;
        }
        if (variables.GetCurrentHealthPoints() <= 0 && !died)
        {
            FindObjectOfType<AudioManager>().Play(deathSounds[Random.Range(0, deathSounds.Length + 1)]);
            died = true;
            
        }

        if (!PlayerMovement.IsDodging)
        {
            FindObjectOfType<AudioManager>().Play(hurtSounds[Random.Range(0, hurtSounds.Length + 1)]);

        }
    }
}
