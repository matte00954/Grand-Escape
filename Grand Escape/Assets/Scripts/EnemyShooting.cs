using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] GameObject gun;
    [SerializeField] GameObject barrelEnd;
    [SerializeField] GameObject enemyAmmo;
    Transform playerTransform;

    [SerializeField] LayerMask aimCollisionMask;

    [SerializeField] float enemyRange;
    [SerializeField] float reloadTimeInSeconds;

    float rotationSpeedMultiplier;
    float reloadTimer;

    bool isAlerted = false;

    // Animations
    private Animator anim;

    void Start()
    {
        // Animations
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;

        if (isAlerted)
        {
            RotateSelfToTarget();
            TakeAim();
            RotateGun();
        }
        else if (!isAlerted && gun.transform.rotation != Quaternion.identity)
            RotateGun();

        if (reloadTimer > 0f)
            reloadTimer -= Time.deltaTime;
    }

    private void RotateGun()
    {
        if (isAlerted)
        {
            Vector3 direction = (playerTransform.position - gun.transform.position).normalized;
            //Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            gun.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z)); //Quaternion.Slerp(gun.transform.rotation, lookRotation, Time.deltaTime * rotationSpeedMultiplier);
            //Debug.Log("Trying to rotate:" + lookRotation);
            gun.transform.rotation = Quaternion.Euler(gun.transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        }
        //else
        //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.identity, Time.deltaTime * rotationSpeedMultiplier);
    }

    private void TakeAim()
    {
        RaycastHit hit;

        if (isAlerted && Physics.Raycast(barrelEnd.transform.position, barrelEnd.transform.forward, out hit, enemyRange, aimCollisionMask) && reloadTimer <= 0f)
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                reloadTimer = reloadTimeInSeconds;
                ShootWithGun();
            }
        }
    }

    private void RotateSelfToTarget()
    {
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeedMultiplier);
    }

    public void SetAlert(bool isAlerted)
    {
        this.isAlerted = isAlerted;
    }

    private void ShootWithGun()
    {
        Instantiate(enemyAmmo, barrelEnd.transform.position, barrelEnd.transform.rotation);

        // Animations
        anim.SetTrigger("Fire");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(barrelEnd.transform.position, barrelEnd.transform.forward * enemyRange);
    }
}
