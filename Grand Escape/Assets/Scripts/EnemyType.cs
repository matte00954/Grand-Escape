//Author: Mattias Larsson
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy type", menuName = "Enemy")]

public class EnemyType : ScriptableObject
{

    [SerializeField] private int maxHealthPoints = 100;

    [SerializeField] private int damage = 100;

    //public int MaxHealthPoints { get => maxHealthPoints; set => maxHealthPoints = value; }

    //public int Damage { get => damage; set => damage = value; }

    public int GetMaxHealthPoints() { return maxHealthPoints; }

    public int GetDamage() { return damage; }
}