//author: Mattias Larsson
//author: William Örnquist
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform childRespawnTransform; //child object that has a respawn transform that PlayerRespawn object gets moved to

    [SerializeField] private int checkpointIndex;

    [SerializeField] CheckpointRespawnHandler checkpointRespawnHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has reached checkpoint " + "current check point is " + checkpointIndex);

            other.gameObject.GetComponentInParent<PlayerVariables>().SetCheckpointIndex(checkpointIndex);
            ChangeRespawn();
            FindObjectOfType<SaveAndLoadData>().Save();
            this.gameObject.SetActive(false);
        }
    }

    private void ChangeRespawn() //After player reaches this checkpoint
    {
        checkpointRespawnHandler.SetNewRespawnPoint(childRespawnTransform.position);
    }
}