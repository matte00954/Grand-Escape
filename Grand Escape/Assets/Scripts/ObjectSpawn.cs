using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [Header("Objects that spawns when you reach this point")] //For optimising
    [SerializeField] GameObject[] gameObjects;

    [Header("Objects that gets disabled when you reach this point")] //For optimising 
    [SerializeField] GameObject[] toDisable;

    private void Start()
    {
        DisableObjects(gameObjects);

        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i] == null)
            {
                Debug.LogError(i + " in gameObjects array is null " + this.gameObject);
            }
        }
    }

    public void SpawnObjects()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(true);
        }
    }

    public void DisableObjects(GameObject[] list) //may not need to be public, but might use this one for something else
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i].SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnObjects();
            DisableObjects(toDisable);
        }
    }
}
