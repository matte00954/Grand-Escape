//author: Mattias Larsson
//author: William �rnquist
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private readonly Transform childRespawnTransform; //child object that has a respawn transform that PlayerRespawn object gets moved to

    [SerializeField] private readonly int checkpointIndex;

    private CheckpointRespawnHandler checkpointRespawnHandler;

    private void Start()
    {
        checkpointRespawnHandler = FindObjectOfType<CheckpointRespawnHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player has reached checkpoint " + "current check point is " + checkpointIndex);

            other.gameObject.GetComponent<PlayerVariables>().SetCheckpointIndex(checkpointIndex);
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