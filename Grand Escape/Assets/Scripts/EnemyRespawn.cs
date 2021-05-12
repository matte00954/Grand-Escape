using UnityEngine;
using System.Collections.Generic;

public class EnemyRespawn : MonoBehaviour
{
    [Header("Enemies in previous section to respawn, if player dies")] //All enemies in the previous section that will spawn if player respawns at this location

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();

    private void SetEnemiesInactiveBeforeRespawn() //not used at the moment
    {
        for (int i = 0; i < enemies.Capacity - 1; i++)
            enemies[i].SetActive(false);
    }

    public void RespawnEnemies() 
    {
        if (this.gameObject.activeSelf)
        {
            for (int i = 0; i < enemies.Capacity; i++)
            {
                if (enemies[i] == null)
                    Debug.LogError(i + " in enemies list is null " + this.gameObject);
                else
                {
                    enemies[i].GetComponent<EnemyVariables>().ResetAllStats();
                    enemies[i].GetComponent<EnemyVariables>().ResetPosition();
                    enemies[i].SetActive(true);
                }
            }
        }
    }
}
