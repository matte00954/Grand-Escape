using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    GameObject meelePrefab;

    GameObject rangedPrefab;

    Transform[] enemiesTransform;

    private void Start()
    {
        enemiesTransform = new Transform[enemies.Length];

        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            enemiesTransform[i] = enemies[i].gameObject.transform;
        }

        //for (int i = 0; i <= enemies.Length - 1; i++)
        //{
        //}
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RespawnEnemies();
        }
    }

    private void DestroyEnemiesBeforeRespawn()
    {
        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            enemies[i].SetActive(false);
        }
    }

    public void RespawnEnemies() //TODO event system???
    {
        DestroyEnemiesBeforeRespawn();
        for (int i = 0; i <= enemies.Length - 1; i++)
        {

            enemies[i].SetActive(true);
        }
    }
}
