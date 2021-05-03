using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnHandler : MonoBehaviour
{
    [SerializeField] GameObject[] allRespawnPoints;

    public void RepsawnAll()
    {
        for (int i = 0; i <= allRespawnPoints.Length - 1; i++)
        {
            if (allRespawnPoints[i] == null)
            {
                Debug.LogError(i + " in the game manager is null");
                break;
            }
            allRespawnPoints[i].GetComponent<EnemyRespawn>().RespawnEnemies();
            Debug.Log(allRespawnPoints[i] + " has respawned enemies");
        }
    }
}
