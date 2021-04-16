using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] EnemyMovement enemyMovement;
    [SerializeField] GameObject gun;
    [SerializeField] GameObject barrelEnd;
    [SerializeField] GameObject enemyAmmo;

    [SerializeField] LayerMask layerMask;

    [SerializeField] float enemyRange;
    [SerializeField] float reloadTimeInSeconds;

    float rotationSpeedMultiplier;
    float reloadTimer;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GunVerticalPositioning();

        if (reloadTimer <= reloadTimeInSeconds)
            reloadTimer += Time.deltaTime;

        //if (enemyMovement.GetTarget().transform)
        //{

        //}
    }

    private void GunVerticalPositioning()
    {
        /*Vector3 direction = (enemyMovement.GetTarget().transform.position - gun.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, direction.x, 0));

        gun.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeedMultiplier);*/

        gun.transform.LookAt(enemyMovement.GetTarget().transform);

        RaycastHit hit;

        if (Physics.Raycast(barrelEnd.transform.position, barrelEnd.transform.forward, out hit, enemyRange, layerMask) && reloadTimer >= reloadTimeInSeconds)
        {
            if (hit.collider.CompareTag("Player"))
            {
                reloadTimer = 0;
                ShootWithGun();
            }
        }
    }

    private void ShootWithGun()
    {
        Instantiate(enemyAmmo, barrelEnd.transform.position, barrelEnd.transform.rotation);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(barrelEnd.transform.position, barrelEnd.transform.forward * enemyRange);
    }
}
