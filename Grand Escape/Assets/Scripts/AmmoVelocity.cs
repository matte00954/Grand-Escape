using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoVelocity : MonoBehaviour
{
    public LayerMask terrainMask; //Ground mask / terrain mask
    public LayerMask targetMask; //The target layer the projectile is searching to damage
    public Transform bulletHasHitCheck;

    private Vector3 direction;

    public float bulletCollisionDetectionDistance = 0.4f;
    public float timeUntilBulletDestroyed = 0.05f;
    public float speed;
    public float bulletDoesNotHitTimer;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;

        StartCoroutine(TimeToDestroy(bulletDoesNotHitTimer));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 6:
                if (targetMask == LayerMask.GetMask("Player"))
                {
                    other.gameObject.GetComponentInParent<PlayerVariables>().ApplyDamage(damage);
                    StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
                }
                break;
            case 8:
                if (targetMask.value == LayerMask.GetMask("Enemy"))
                    other.gameObject.GetComponent<EnemyVariables>().ApplyDamage(damage);
                StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
                break;
            case 7:
                Debug.Log("Bullet has impacted ground");
                StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
                break;
            case 11:
                Debug.Log("Bullet has impacted wall");
                StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
                break;
            case 0:
                break;
            default:
                Debug.LogError("ERROR: Bullet could not find the appropriate layer from the hit target. Returned layer nr is: " + other.gameObject.layer);
                break;
        }
    }

    IEnumerator TimeToDestroy(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy); //Om kulan inte träffar något så förstörs den efter 10 sekunder
        Destroy(this.gameObject);

        //TODO
        //olika vapen bör ha olika timeToDestroy
    }
}
