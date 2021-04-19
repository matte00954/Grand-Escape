using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons")]
public class Weapons : ScriptableObject
{
    [SerializeField] MeshFilter weaponMeshFilter;
    [SerializeField] MeshRenderer weaponMeshRenderer;
    [SerializeField] Sprite crosshair; //crosshair on UI

    [SerializeField] string weaponName; //musket/pistol

    [SerializeField] int weaponDamage = 100;
    [SerializeField] int ammoCapacity; //how many bullets can fit in the gun

    [SerializeField] float effectiveRange; //how long until bullet gets destroyed if it does not hit
    [SerializeField] float reloadTime; //In seconds

    Transform weaponTransform;

    public MeshFilter GetWepMeshFilter() { return weaponMeshFilter; }

    public MeshRenderer GetWepMeshRenderer() { return weaponMeshRenderer; }

    public Sprite GetWepSprite() { return crosshair; }

    public string GetWepName() { return weaponName; }

    public int GetWepDmg() { return weaponDamage; }

    public int GetAmmoCap() { return ammoCapacity; }

    public float GetEffectiveRange() { return effectiveRange; }

    public float GetReloadTime() { return reloadTime; }

}
