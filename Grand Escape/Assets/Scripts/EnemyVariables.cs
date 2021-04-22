using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{

    int healthPoints = 100;

    //Dessa b�r nog vara i en annan klass i framtiden
    //private float resistanceMeele = 0;

    //private float resistanceRanged = 0;

    //OBS TEMPOR�R KOD, m�ste f�rb�ttras detta b�r finnas i typ en klass som f�rvara dessa typer av v�rden
    float damageFromBullets = 100;
    float damageFromSword = 100;

    //OBS TEMPOR�R KOD, m�ste f�rb�ttras
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
            //OBS Tempor�r kod
            Destroy(this.gameObject);
        }

    }

    private void OnTriggerEnter(Collider other) //Bara D�ligt
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
