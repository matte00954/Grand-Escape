using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnHandler : MonoBehaviour
{
    [Header("All checkpoints in the scene here")] 
    [SerializeField] GameObject[] allCheckPoints; //all checkpoints in the scene


    private void Start()
    {
        if (allCheckPoints.Length == 0)
        {
            Debug.LogError("Checkpoint list in game manager is empty, enemies will not respawn");
        }

        for (int i = 0; i <= allCheckPoints.Length - 1; i++)
        {
            if (allCheckPoints[i] == null)
            {
                Debug.LogError(i + " in the game manager is null, ALL checkpoints need to be assigned to the game manager");
            }
        }
    }

    public void RepsawnAll()
    {
        for (int i = 0; i <= allCheckPoints.Length - 1; i++)
        {
            if (allCheckPoints[i] == null)
            {
                Debug.LogError(i + " in the game manager is null");
            }
            allCheckPoints[i].GetComponent<EnemyRespawn>().RespawnEnemies();
            Debug.Log(allCheckPoints[i] + " has respawned enemies");
        }
    }
}
