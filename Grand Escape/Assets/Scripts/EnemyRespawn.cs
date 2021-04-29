using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawn : MonoBehaviour
{
    [SerializeField] GameObject[] enemies;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RespawnEnemies();
        }
    }

    public void RespawnEnemies() //TODO event system???
    {
        foreach (GameObject enemy in enemies)
        {
            enemy.SetActive(true);
        }
    }
}
