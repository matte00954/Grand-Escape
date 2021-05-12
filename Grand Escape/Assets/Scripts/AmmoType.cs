using UnityEngine;

[CreateAssetMenu(fileName = "New Ammo Type", menuName = "Ammo")]
public class AmmoType : ScriptableObject
{
    //[Header("DO NOT USE")]
    //[SerializeField] LayerMask terrainMask; //Ground mask / terrain mask, NOT USED
    //[SerializeField] LayerMask targetMask; //The target layer the projectile is searching to damage, NOT USED


    [SerializeField] string ammoName;
    [SerializeField] int ammoDamage;
    [SerializeField] float speed;
    [SerializeField] float bulletLifetime;

    //public LayerMask GetTerrainMask() { return terrainMask; }

    //public LayerMask GetTargetMask() { return targetMask; }


    //Keeping these until all method calls are changed!!!
    public string GetAmmoName() { return ammoName; } 
    public int GetAmmoDamage() { return ammoDamage; }
    public float GetAmmoSpeed() { return speed; }
    public float GetBulletLifetime() { return bulletLifetime; }
}