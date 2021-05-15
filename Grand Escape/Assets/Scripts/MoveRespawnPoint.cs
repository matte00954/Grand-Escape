using UnityEngine;

public class MoveRespawnPoint : MonoBehaviour
{
    public void SetNewPoint(Vector3 newPoint)
    {
        this.gameObject.transform.position = newPoint;
        Debug.Log("Respawn point move to " + newPoint);
    }
}

