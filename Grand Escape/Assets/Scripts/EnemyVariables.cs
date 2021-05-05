using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVariables : MonoBehaviour
{

    [SerializeField] EnemyType enemyType;
    [SerializeField] ParticleSystem deathParticleEffect;
    [SerializeField] AudioClip[] deathClips;

    int healthPoints;
    private AudioSource audioSource;

    Vector3 startPosition;

    private void Start()
    {
        ResetAllStats();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthPoints <= 0)
        {
            Debug.Log("Enemy dies");
            ParticleSystem particleSystem = Instantiate(deathParticleEffect);
            particleSystem.transform.position = transform.position;
            AudioClip clip = deathClips[Random.Range(0, deathClips.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);
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

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}