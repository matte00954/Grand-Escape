using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{

    int healthPoints = 100;

    //Dessa bör nog vara i en annan klass i framtiden
    //private float resistanceMeele = 0;

    //private float resistanceRanged = 0;

    //OBS TEMPORÄR KOD, måste förbättras detta bör finnas i typ en klass som förvara dessa typer av värden
    float damageFromBullets = 100;
    float damageFromSword = 100;

    //OBS TEMPORÄR KOD, måste förbättras
    //[Header("TEST Variabler")]
    //public bool isSniper;
    //public bool isMeele;

    private void Awake()
    {

        /*if (isSniper)
        {
            resistanceRanged = 0.5f;
        }
        if (isMeele)
        {
            resistanceMeele = 0.5f;
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        if(healthPoints <= 0)
        {
            Debug.Log("Enemy dies");
            //OBS Temporär kod
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other) //Bara Dåligt
    {
        if (other.gameObject.CompareTag("PlayerMelee"))
        {
            ApplyDamage(damageFromSword);
        }
    }

    public void ApplyDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " in damage");
        healthPoints -= (int)damage;
    }

}
