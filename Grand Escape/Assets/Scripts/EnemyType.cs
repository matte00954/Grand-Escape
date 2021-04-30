using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New enemy type", menuName = "Enemy")]

public class EnemyType : ScriptableObject
{
    [SerializeField] string enemyType;

    [SerializeField] int maxHealthPoints = 100;

    public string GetEnemyType() { return enemyType; }

    public int GetMaxHealthPoints() { return maxHealthPoints; }
}