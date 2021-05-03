using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons")]
public class Weapons : ScriptableObject
{

    [Header("DO NOT USE")]
    [SerializeField] MeshFilter weaponMeshFilter; //Not sure if needed, NOT USED
    [SerializeField] MeshRenderer weaponMeshRenderer; //Not sure if needed, NOT USED
    Transform weaponTransform; //might not be used

    [SerializeField] Sprite crosshair; //crosshair on UI

    [SerializeField] string weaponName; //musket/pistol
    [SerializeField] int weaponDamage = 100; //Should be 100, unless changed design decision
    [SerializeField] int ammoCapacity = 1; //how many bullets can fit in the gun, should be 1
    [SerializeField] float reloadTime; //In seconds
    [SerializeField] bool canZoom = true; //Can you zoom with weapon? default true

    public MeshFilter GetWepMeshFilter() { return weaponMeshFilter; }

    public MeshRenderer GetWepMeshRenderer() { return weaponMeshRenderer; }

    public Sprite GetWepSprite() { return crosshair; }

    public string GetWepName() { return weaponName; }

    public int GetWepDmg() { return weaponDamage; }

    public int GetAmmoCap() { return ammoCapacity; }

    public float GetReloadTime() { return reloadTime; }

    public bool GetCanZoom() { return canZoom; }

}
