//Main author: Mattias Larsson
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [Header("Objects that spawn")]
    [Tooltip("These objects will be set active once the player reaches the trigger connected to this gameObject(checkpoint)")]
    [SerializeField] private GameObject[] objectList;

    private void Start()
    {
        DisableObjects(objectList);
    }

    public void SpawnObjects()
    {
        for (int i = 0; i < objectList.Length; i++)
            objectList[i].SetActive(true);
    }

    private void DisableObjects(GameObject[] list) 
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
