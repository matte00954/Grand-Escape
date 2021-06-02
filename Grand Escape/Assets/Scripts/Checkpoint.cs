//author: Mattias Larsson
//author: William Örnquist
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform childRespawnTransform; //child object that has a respawn transform that PlayerRespawn object gets moved to

    [SerializeField] private int checkpointIndex;

    private bool isActive = true;

    [SerializeField] CheckpointRespawnHandler checkpointRespawnHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && isActive)
        {
            Debug.Log("Player has reached checkpoint " + "current check point is " + checkpointIndex);

            other.gameObject.GetComponentInParent<PlayerVariables>().SetCheckpointIndex(checkpointIndex);
            ChangeRespawn();
            FindObjectOfType<SaveAndLoadData>().Save();
            isActive = false;

            foreach (MonoBehaviour monoBehaviour in GetComponents<MonoBehaviour>())
            {
                monoBehaviour.enabled = false;
            }
        }
    }

    private void ChangeRespawn() //After player reaches this checkpoint
    {
        checkpointRespawnHandler.SetNewRespawnPoint(childRespawnTransform.position);
    }
}