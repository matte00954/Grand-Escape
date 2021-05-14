using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [Header("Objects that spawns when you reach this point")] //For optimising
    [SerializeField] private GameObject[] gameObjects;

    [Header("Objects that gets disabled when you reach this point")] //For optimising 
    [SerializeField] private GameObject[] toDisable;

    private void Start()
    {
        DisableObjects(gameObjects);
    }

    public void SpawnObjects()
    {
        for (int i = 0; i < gameObjects.Length; i++)
            gameObjects[i].SetActive(true);
    }

    public void DisableObjects(GameObject[] list) //may not need to be public, but might use this one for something else
    {
        for (int i = 0; i < list.Length; i++)
            list[i].SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpawnObjects();
        }
    }
}
