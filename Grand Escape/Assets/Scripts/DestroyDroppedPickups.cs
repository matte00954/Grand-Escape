//Main author: Mattias Larsson
using UnityEngine;

public class DestroyDroppedPickups : MonoBehaviour
{
    void Update() //Deletes dropped pickups on respawn
    {
        if (!PlayerVariables.isAlive)
        {
            Destroy(this.gameObject);
        }
    }
}
