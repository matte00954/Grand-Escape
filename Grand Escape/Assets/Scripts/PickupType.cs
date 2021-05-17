//Author: Mattias Larsson
using UnityEngine;

[CreateAssetMenu(fileName = "New pickup type", menuName = "Pickup")]

public class PickupType : ScriptableObject
{
    [SerializeField] private string pickupType;

    [SerializeField] private int amount;

    public string GetPickupType()
    {
        return pickupType;
    }

    public int GetAmount()
    {
        return amount;
    }
}
