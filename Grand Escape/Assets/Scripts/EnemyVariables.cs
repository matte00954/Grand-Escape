using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{

    [SerializeField] EnemyType enemyType;
    [SerializeField] ParticleSystem deathParticleEffect;

    int healthPoints;

    private void Start()
    {
        ResetAllStats();
    }

    // Update is called once per frame
    void Update()
    {
        if(healthPoints <= 0)
        {
            Debug.Log("Enemy dies");
            ParticleSystem particleSystem = Instantiate(deathParticleEffect);
            particleSystem.transform.position = transform.position;
            this.gameObject.SetActive(false);
        }
    }

    public void ApplyDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " in damage");
        healthPoints -= (int)damage;
    }

    public void ResetAllStats()
    {
        healthPoints = enemyType.GetMaxHealthPoints();
    }
}