//Author: Mattias Larsson
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Ammo")]
public class AmmoType : ScriptableObject
{
    //[Header("DO NOT USE")]
    //[SerializeField] LayerMask terrainMask; //Ground mask / terrain mask, NOT USED
    //[SerializeField] LayerMask targetMask; //The target layer the projectile is searching to damage, NOT USED
    //public LayerMask GetTerrainMask() { return terrainMask; }

    //public LayerMask GetTargetMask() { return targetMask; }


    [SerializeField] private string ammoName;
    [SerializeField] private int ammoDamage;
    [SerializeField] private float speed;
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float horizontalAccuracyMargin; //Represented in degrees
    [SerializeField] private float verticalAccuracyMargin; //Represented in degrees

    public string GetAmmoName() { return ammoName; } 
    public int GetAmmoDamage() { return ammoDamage; }
    public float GetAmmoSpeed() { return speed; }
    public float GetBulletLifetime() { return bulletLifetime; }
    public float GetHorizontalMargin() { return horizontalAccuracyMargin; }
    public float GetVerticalMargin() { return verticalAccuracyMargin; }
}