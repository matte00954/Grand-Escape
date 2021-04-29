using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Ammo")]
public class AmmoType : ScriptableObject
{
    [SerializeField] LayerMask terrainMask; //Ground mask / terrain mask

    [SerializeField] LayerMask targetMask; //The target layer the projectile is searching to damage

    [SerializeField] string ammoName;

    [SerializeField] int ammoDamage;

    [SerializeField] float speed;

    [SerializeField] float bulletDoesNotHitTimer;

    public LayerMask GetTerrainMask() { return terrainMask; }

    public LayerMask GetTargetMask() { return targetMask; }

    public string GetAmmoName() { return ammoName; }

    public int GetAmmoDamage() { return ammoDamage; }

    public float GetAmmoSpeed() { return speed; }

    public float GetBulletDoesNotHitTimer() { return bulletDoesNotHitTimer; }
}