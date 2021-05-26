//Main author: William Örnquist
//Secondary author: Mattias Larsson
using UnityEngine;

public class AmmoVelocity : MonoBehaviour
{
    //All other variables move to AmmoType Scriptable object

    [SerializeField] private AmmoType ammo;
    [SerializeField] private LayerMask collisionMask;

    private Vector3 direction;
    private float lifeTimer;
    private bool isActive = true;

    private void Awake()
    {
        float horizontalMargin = ammo.GetHorizontalMargin();
        float verticalMargin = ammo.GetVerticalMargin();
        float randomHorizontalError = Random.Range(-horizontalMargin, horizontalMargin);
        float randomVerticalError = Random.Range(-verticalMargin, verticalMargin);
        transform.Rotate(new Vector3(randomVerticalError, randomHorizontalError, 0f));
        direction = transform.forward;
        Debug.Log("Bullet direction: " + direction);
        Debug.Log("Bullet rotation: " + transform.rotation.eulerAngles);
        lifeTimer = ammo.GetBulletLifetime();
    }

    private void Update()
    {
        RayCheck();
        transform.position += direction * ammo.GetAmmoSpeed() * Time.deltaTime;

        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0f)
            Destroy(this.gameObject);
    }

    private void RayCheck()
    {
        if (Physics.Raycast(transform.position, direction, out RaycastHit raycastHit, ammo.GetAmmoSpeed() * Time.deltaTime, collisionMask) && isActive)
        {
            isActive = false;
            if (raycastHit.collider.gameObject.CompareTag("Player"))
                raycastHit.collider.gameObject.GetComponentInParent<PlayerVariables>().ApplyDamage(ammo.GetAmmoDamage());
            else if (raycastHit.collider.gameObject.CompareTag("Enemy"))
                raycastHit.collider.gameObject.GetComponent<EnemyVariables>().ApplyDamage(ammo.GetAmmoDamage());

            Destroy(this.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, direction * ammo.GetAmmoSpeed() * Time.deltaTime);
    }
}
