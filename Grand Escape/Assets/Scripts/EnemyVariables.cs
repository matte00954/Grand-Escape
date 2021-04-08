using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{

    private int healthPoints = 100;

    //Dessa b�r nog vara i en annan klass i framtiden
    //private float resistanceMeele = 0;

    //private float resistanceRanged = 0;

    //OBS TEMPOR�R KOD, m�ste f�rb�ttras detta b�r finnas i typ en klass som f�rvara dessa typer av v�rden
    private float damageFromBullets = 100;
    private float damageFromSword = 100;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerBullets"))
        {
            ApplyDamage(damageFromBullets);
        }
        if (other.gameObject.CompareTag("PlayerMeele"))
        {
            ApplyDamage(damageFromSword);
        }
    }

    private void ApplyDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " in damage");
        healthPoints -= (int)damage;
    }

}
