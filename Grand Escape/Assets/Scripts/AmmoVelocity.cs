using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoVelocity : MonoBehaviour
{
    public LayerMask terrainMask; //Ground mask / terrain mask
    public LayerMask enemyMask;
    public Transform bulletHasHitCheck;

    private Vector3 direction;
    private Transform cameraTransform;
    private bool hasHitAnything;

    public float bulletCollisionDetectionDistance = 0.4f;
    public float timeUntilBulletDestroyed = 0.05f;
    public float speed;
    public float bulletDoesNotHitTimer;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;

        direction = cameraTransform.forward;

        hasHitAnything = false;

        StartCoroutine(TimeToDestroy(bulletDoesNotHitTimer));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        bool hasHitObject = Physics.CheckSphere(bulletHasHitCheck.position, bulletCollisionDetectionDistance, terrainMask);
        bool hasHitEnemy = Physics.CheckSphere(bulletHasHitCheck.position, bulletCollisionDetectionDistance, enemyMask);

        if (!hasHitAnything) //ser till att det under inte körs flera gånger
        {
            if (hasHitObject)
            {
                Debug.Log("Bullet has impacted terrain");
                hasHitAnything = true;
                StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
            }
            if (hasHitEnemy)
            {
                Debug.Log("Bullet has impacted enemy");
                hasHitAnything = true;
                StartCoroutine(TimeToDestroy(timeUntilBulletDestroyed));
            }
            //vi kan behöva fixa en OnTriggerEnter funktion här i framtiden, för att hantera när/hur fiender blir träffade av dessa kulor
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
