using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnHandler : MonoBehaviour
{
    [Header("All checkpoints in the scene here")] 
    [SerializeField] List<EnemyRespawn> enemyRespawnList = new List<EnemyRespawn>();


    private void Start()
    {
        if (enemyRespawnList.Capacity == 0)
        {
            Debug.LogError("Checkpoint list in game manager is empty, enemies will not respawn");
        }

        for (int i = 0; i < enemyRespawnList.Capacity; i++)
        {
            if (enemyRespawnList[i] == null)
            {
                Debug.LogError(i + " in the game manager is null, ALL checkpoints need to be assigned to the game manager");
            }
        }
    }

    public void RepsawnAll()
    {
        for (int i = 0; i < enemyRespawnList.Capacity; i++)
        {
            if (enemyRespawnList[i] == null)
            {
                Debug.LogError(i + " in the game manager is null");
            }
            else
            {
                enemyRespawnList[i].GetComponent<EnemyRespawn>().RespawnEnemies();
                Debug.Log(enemyRespawnList[i] + " has respawned enemies");
            }
        }
    }
}
