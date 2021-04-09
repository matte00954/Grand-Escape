using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoVelocity : MonoBehaviour
{
    public LayerMask terrainMask; //Ground mask / terrain mask
    public LayerMask targetMask; //The target layer the projectile is searching to damage
    public Transform bulletHasHitCheck;

    private Vector3 direction;
    private Transform startTransform;
    private bool hasHitAnything;

    public float bulletCollisionDetectionDistance = 0.4f;
    public float timeUntilBulletDestroyed = 0.05f;
    public float speed;
    public float bulletDoesNotHitTimer;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        switch (targetMask.value)
        {
            case 6: //Target player
                startTransform = transform;
                break;
            case 8: //Target enemy
                startTransform = GameObject.Find("Main Camera").transform;
                break;
            default:
                Debug.LogError("ERROR: The selected TargetMask " + targetMask.value + ", does not match any cases.");
                break;
        }

        direction = startTransform.forward;

        hasHitAnything = false;

        StartCoroutine(TimeToDestroy(bulletDoesNotHitTimer));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        //bool hasHitObject = Physics.CheckSphere(bulletHasHitCheck.position, bulletCollisionDetectionDistance, terrainMask);
        //bool hasHitTarget = Physics.CheckSphere(bulletHasHitCheck.position, bulletCollisionDetectionDistance, targetMask);

        //if (!hasHitAnything) //ser till att det under inte körs flera gånger
        //{
        //    if (hasHitObject)
        //    {
        //        Debug.Log("Bullet has impacted terrain");
        //        hasHitAnything = true;
        //        StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
        //    }
        //    if (hasHitTarget)
        //    {
        //        Debug.Log("Bullet has impacted target");
        //        hasHitAnything = true;
        //        StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
        //    }
            //vi kan behöva fixa en OnTriggerEnter funktion här i framtiden, för att hantera när/hur fiender blir träffade av dessa kulor
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.layer)
        {
            case 6:
                if (targetMask.value == 6)
                {
                    other.gameObject.GetComponent<PlayerVariables>().ApplyDamage(damage);
                    StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
                }
                break;
            case 8:
                if (targetMask.value == 8)
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
