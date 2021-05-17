using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRespawn : MonoBehaviour
{
    [Header("Objects in previous section to respawn, if player dies")] //All enemies in the previous section that will spawn if player respawns at this location

    [SerializeField] private List<GameObject> objectsToRespawn = new List<GameObject>();

    private void SetEnemiesInactiveBeforeRespawn() //not used at the moment
    {
        for (int i = 0; i < objectsToRespawn.Count - 1; i++)
            objectsToRespawn[i].SetActive(false);
    }

    public void RespawnEnemies()
    {
        if (gameObject.activeSelf)
        {
            for (int i = 0; i < objectsToRespawn.Count; i++)
            {
                if (objectsToRespawn[i] == null)
                    Debug.LogError(i + " in objectsToRespawn list is null " + gameObject);
                
                if(!objectsToRespawn[i].activeInHierarchy)
                {
                    objectsToRespawn[i].SetActive(true);
                }
            }
        }
    }
}
