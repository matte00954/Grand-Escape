using System.Collections;
using UnityEngine;

public class AmmoVelocity : MonoBehaviour
{
    //All other variables move to AmmoType Scriptable object

    [SerializeField] AmmoType ammo;
    [SerializeField] LayerMask collisionMask;

    private Vector3 direction;
    float lifeTimer;

    void Start()
    {
        direction = transform.forward;
    }

    private void Awake()
    {
        lifeTimer = ammo.GetBulletLifetime();
    }

    void Update()
    {
        RayCheck();
        transform.position += direction * ammo.GetAmmoSpeed() * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
            Destroy(this.gameObject);
    }

    private void RayCheck()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, direction, out raycastHit, ammo.GetAmmoSpeed() * Time.deltaTime, collisionMask))
        {
            if (raycastHit.collider.gameObject.tag == "Player")
                raycastHit.collider.gameObject.GetComponentInParent<PlayerVariables>().ApplyDamage(ammo.GetAmmoDamage());
            else if (raycastHit.collider.gameObject.tag == "Enemy")
                raycastHit.collider.gameObject.GetComponent<EnemyVariables>().ApplyDamage(ammo.GetAmmoDamage());

            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, direction * ammo.GetAmmoSpeed() * Time.deltaTime);
    }
}
