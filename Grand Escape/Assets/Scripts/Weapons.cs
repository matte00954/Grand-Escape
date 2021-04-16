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
}
