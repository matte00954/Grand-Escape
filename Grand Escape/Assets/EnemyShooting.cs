using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public EnemyMovement enemyMovement;

    public GameObject gun;

    public GameObject barrelEnd;

    public GameObject enemyAmmo;

    public float enemyRange;

    private float rotationSpeedMultiplier;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        GunVerticalPositioning();

        if (enemyMovement.GetTarget().transform)
        {

        }
    }

    private void GunVerticalPositioning()
    {
        /*Vector3 direction = (enemyMovement.GetTarget().transform.position - gun.transform.position).normalized;

        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(0, direction.x, 0));

        gun.transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeedMultiplier);*/

        gun.transform.LookAt(enemyMovement.GetTarget().transform);

        gun.transform.Rotate(Vector3.right * 90);
    }

    private void ShootWithGun()
    {
        Instantiate(enemyAmmo,barrelEnd.transform);
    }
}
