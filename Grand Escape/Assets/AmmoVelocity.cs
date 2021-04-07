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

    public float bulletCollisionDetectionDistance = 0.4f;
    public float speed;
    public float bulletDoesNotHitTimer;

    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = GameObject.Find("Main Camera").transform;

        direction = cameraTransform.forward;

        StartCoroutine(TimeToDestroy(bulletDoesNotHitTimer));
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;

        bool hasHitObject = Physics.CheckSphere(bulletHasHitCheck.position, bulletCollisionDetectionDistance, terrainMask);
        bool hasHitEnemy = Physics.CheckSphere(bulletHasHitCheck.position, bulletCollisionDetectionDistance, enemyMask);

        if(hasHitObject || hasHitEnemy) //Ett problem är att denna körs flera gånger, kommer ej på en bra lösning för tillfället
        {
            Debug.Log("Bullet has impacted");
            StartCoroutine(TimeToDestroy(0.1f));
        }
        //vi kan behöva fixa en OnTriggerEnter funktion här i framtiden, för att hantera när/hur fiender blir träffade av dessa kulor
    }

    IEnumerator TimeToDestroy(float timeToDestroy)
    {
        yield return new WaitForSeconds(timeToDestroy); //Om kulan inte träffar något så förstörs den efter 10 sekunder
        Destroy(this.gameObject);

        //TODO
        //olika vapen bör ha olika timeToDestroy
    }
}
