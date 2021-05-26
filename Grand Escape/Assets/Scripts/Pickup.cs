//Main author: Mattias Larsson
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] PickupType pickupType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            other.gameObject.GetComponent<PlayerVariables>().AddStatAfterPickup(pickupType.GetPickupType(), pickupType.GetAmount());

            AudioManager audioManager = FindObjectOfType<AudioManager>();

            audioManager.Play(pickupType.GetPickupSoundName());

            gameObject.SetActive(false);
        }
    }
}
