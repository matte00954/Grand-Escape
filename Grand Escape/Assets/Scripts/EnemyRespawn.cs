//Author: Mattias Larsson
using UnityEngine;
using System.Collections.Generic;

public class EnemyRespawn : MonoBehaviour
{
    [Header("Enemies in previous section to respawn, if player dies")] //All enemies in the previous section that will spawn if player respawns at this location

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    public void RespawnEnemies() 
    {
        if (gameObject.activeSelf)
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].GetComponent<EnemyVariables>().ResetAllStats();
                enemies[i].GetComponent<EnemyVariables>().ResetPosition();
            }
    }
}
