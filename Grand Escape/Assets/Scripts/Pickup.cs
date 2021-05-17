using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] PickupType pickupType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            other.gameObject.GetComponent<PlayerVariables>().AddingStatAfterPickup(pickupType.GetPickupType(), pickupType.GetAmount());

            gameObject.SetActive(false);
        }
    }
}
