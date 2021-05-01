using UnityEngine;
using UnityEngine.Events;

public class EnemyRespawn : MonoBehaviour
{
    [Header("Enemies in next section to respawn, if player dies")] //All enemies in the next section that will spawn if player respawns at this location
    [SerializeField] GameObject[] enemies;

    [SerializeField] UnityEvent PlayerDies;

    //Transform[] enemiesTransform; //Might change system to instasiate instead of set active

    //GameObject meelePrefab;

    //GameObject rangedPrefab;

    //Code above might be used for rework

    private void Start()
    {

        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            if (enemies[i] == null)
            {
                Debug.LogError(i + " in enemies array is null " + this.gameObject);
            }
        }

        //Do not remove

        //enemiesTransform = new Transform[enemies.Length];

        //for (int i = 0; i <= enemies.Length - 1; i++)
        //{
        //    enemiesTransform[i] = enemies[i].gameObject.transform;
        //}

        //for (int i = 0; i <= enemies.Length - 1; i++)
        //{
        //}

        //Do not remove
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            RespawnEnemies();
        }
    }

    private void SetEnemiesInactiveBeforeRespawn()
    {
        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            enemies[i].SetActive(false);
        }
    }

    public void RespawnEnemies() //TODO event system???
    {
        SetEnemiesInactiveBeforeRespawn();

        for (int i = 0; i <= enemies.Length - 1; i++)
        {
            enemies[i].SetActive(true);
        }
    }
}
