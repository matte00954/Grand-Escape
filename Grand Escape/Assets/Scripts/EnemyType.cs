//Author: Mattias Larsson
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy type", menuName = "Enemy")]

public class EnemyType : ScriptableObject
{
    [SerializeField] private int maxHealthPoints = 100;
    [Tooltip("The amount of stamina the player gains from killing this enemy type."),
        SerializeField] private float staminaLeechAmount = 15f;

    public int GetMaxHealthPoints() { return maxHealthPoints; }
    public float GetStaminaLeechAmount() { return staminaLeechAmount; }
}