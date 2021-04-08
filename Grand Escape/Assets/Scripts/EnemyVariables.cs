using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    [Header("Bullet detection")]
    public GameObject PlayerBulletPrefab;
    public GameObject PlayerSword;

    private Rigidbody thisEnemyRigidbody;
    public float bulletCollisionDetectionRadius = 0.4f;

    private int healthPoints = 100;
    

    //Dessa bör nog vara i en annan klass i framtiden
    private float resistanceMeele = 0;
    private float resistanceRanged = 0;

    //OBS TEMPORÄR KOD, måste förbättras detta bör finnas i typ en klass som förvara dessa typer av värden
    private float damageFromBullets = 100;
    private float damageFromSword = 100;

    //OBS TEMPORÄR KOD, måste förbättras
    [Header("TEST Variabler")]
    public bool isSniper;
    public bool isMeele;

    private void Awake()
    {

        thisEnemyRigidbody = this.gameObject.GetComponent<Rigidbody>();
        

        if (isSniper)
        {
            resistanceRanged = 0.5f;
        }
        if (isMeele)
        {
            resistanceMeele = 0.5f;
        }
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

        /*if (thisEnemyRigidbody.
        {
            ApplyDamage(damageFromBullets * resistanceRanged);
        }
        if (Physics.CheckCapsule(thisEnemyTransform.position, bulletCollisionDetectionRadius, PlayerSword))
        {
            ApplyDamage(damageFromBullets * resistanceMeele);
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.Equals(PlayerBulletPrefab))
        {
            ApplyDamage(damageFromBullets * resistanceRanged);
        }
        if (other.gameObject.Equals(PlayerSword))
        {
            ApplyDamage(damageFromSword * resistanceMeele);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.Equals(PlayerBulletPrefab))
        {
            ApplyDamage(damageFromBullets * resistanceRanged);
        }
        if (other.gameObject.Equals(PlayerSword))
        {
            ApplyDamage(damageFromSword * resistanceMeele);
        }
    }

    private void ApplyDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " in damage");
        healthPoints -= (int)damage;
    }

}
