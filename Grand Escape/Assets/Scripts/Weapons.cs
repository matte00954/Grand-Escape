//Author: Mattias Larsson
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons")]
public class Weapons : ScriptableObject
{
    [Header("Stats")]
    [SerializeField] private string weaponName; //musket/pistol
    [SerializeField] private int weaponDamage = 100; //Should be 100, unless changed design decision
    [SerializeField] private int ammoCapacity = 1; //how many bullets can fit in the gun, should be 1
    [SerializeField] private float reloadTime; //In seconds
    [SerializeField] private bool canZoom = true; //Can you zoom with weapon? default true
    [Header("Sounds")]
    [SerializeField] private string weaponClick;
    [SerializeField] private string fireName;
    [SerializeField] private string reloadStartName;
    [SerializeField] private string reloadFinishName;

    //Stats
    public string GetWepName() { return weaponName; }
    public int GetWepDmg() { return weaponDamage; }
    public int GetAmmoCap() { return ammoCapacity; }
    public float GetReloadTime() { return reloadTime; }
    public bool GetCanZoom() { return canZoom; }

    //Sounds
    public string GetSoundWeaponClick() { return weaponClick; }
    public string GetSoundFire() { return fireName; }
    public string GetSoundReloadStart() { return reloadStartName; }
    public string GetSoundReloadFinish() { return reloadFinishName; }

}
