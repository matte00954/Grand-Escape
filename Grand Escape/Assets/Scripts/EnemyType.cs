using UnityEngine;

[CreateAssetMenu(fileName = "New enemy type", menuName = "Enemy")]

public class EnemyType : ScriptableObject
{

    [SerializeField] int maxHealthPoints = 100;

    public int GetMaxHealthPoints() { return maxHealthPoints; }
}