//Main author: Mattias Larsson
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRespawnHandler : MonoBehaviour
    //Handles enemy and pickup respawns
{
    [Header("All checkpoints in the scene here")]
    [SerializeField] private List<GameObject> checkPointList = new List<GameObject>();

    private List<EnemyRespawn> enemyRespawnList = new List<EnemyRespawn>();
    private List<PickupRespawn> pickupRespawnList = new List<PickupRespawn>();

    private void Start()
    {

        for (int i = 0; i < checkPointList.Count; i++)
        {
            enemyRespawnList.Add(checkPointList[i].GetComponent<EnemyRespawn>());
            pickupRespawnList.Add(checkPointList[i].GetComponent<PickupRespawn>());

            if (checkPointList[i] == null)
            {
                Debug.Log(i + "in checkpoint list is empty");
            }
        }


        if (enemyRespawnList.Count == 0)
            Debug.LogError("Checkpoint list in game manager is empty, enemies will not respawn");

        for (int i = 0; i < enemyRespawnList.Count; i++)
            if (enemyRespawnList[i] == null)
                Debug.LogError(i + " in the game manager is null, ALL checkpoints need to be assigned to the game manager");
    }


    public void RepsawnAll()
    {
        for (int i = 0; i < enemyRespawnList.Count; i++)
        {
            if (enemyRespawnList[i] == null)
                Debug.LogError(i + " in the game manager is null");
            else
            {
                enemyRespawnList[i].GetComponent<EnemyRespawn>().RespawnEnemies();
                Debug.Log(enemyRespawnList[i] + " has respawned enemies");
            }
        }

        for (int i = 0; i < pickupRespawnList.Count; i++)
        {
            if (pickupRespawnList[i] == null)
                Debug.LogError(i + " in the game manager is null");
            else
            {
                pickupRespawnList[i].GetComponent<PickupRespawn>().RespawnObjects();
                Debug.Log(pickupRespawnList[i] + " has respawned pickups and other objects in this list");
            }
        }
    }
}
