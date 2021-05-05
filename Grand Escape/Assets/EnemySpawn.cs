using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    [Header("Enemies that spawns in to the game when you reach this point")] //All enemies in the previous section that will spawn if player respawns at this location
    [SerializeField] GameObject[] enemies;

    private void Start()
    {
        DisableEnemies();

        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            if (enemies[i] == null)
            {
                Debug.LogError(i + " in enemies array is null " + this.gameObject);
            }
        }
    }


    public void SpawnEnemies()
    {
        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            enemies[i].GetComponent<EnemyVariables>().ResetAllStats();
            enemies[i].SetActive(true);
        }
    }

    public void DisableEnemies() //may not need to be public, but might use this one for something else
    {
        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            enemies[i].SetActive(false);
        }
    }
}
