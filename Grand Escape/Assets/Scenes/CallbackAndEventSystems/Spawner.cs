using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class Spawner : MonoBehaviour
    {
        public GameObject unitPrefab;

        public delegate void OnUnitSpawnedDelegate(Health health);
        public event OnUnitSpawnedDelegate OnUnitSpawnedListeners;

        [SerializeField] float spawnRadius;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SpawnUnit();
            }
        }

        private void SpawnUnit()
        {
            GameObject gameObject = Instantiate(unitPrefab);

            gameObject.transform.position = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius),
                Random.Range(-spawnRadius, spawnRadius));

            if (OnUnitSpawnedListeners != null)
            {
                OnUnitSpawnedListeners(gameObject.GetComponent<Health>());
            }
        }
    }
}
