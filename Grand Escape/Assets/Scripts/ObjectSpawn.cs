using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [Header("Objects that spawns when you reach this point")] //For optimising
    [SerializeField] GameObject[] gameObjects;

    private void Start()
    {
        DisableObjects();

        for (int i = 0; i <= gameObjects.Length - 1; i++)
        {
            if (gameObjects[i] == null)
            {
                Debug.LogError(i + " in gameObjects array is null " + this.gameObject);
            }
        }
    }

    public void SpawnObjects()
    {
        for (int i = 0; i <= gameObjects.Length - 1; i++)
        {
            gameObjects[i].SetActive(true);
        }
    }

    public void DisableObjects() //may not need to be public, but might use this one for something else
    {
        for (int i = 0; i <= gameObjects.Length - 1; i++)
        {
            gameObjects[i].SetActive(false);
        }
    }
}
