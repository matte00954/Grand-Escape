using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Gameobjects")]
    public GameObject player;
    public LayerMask playerMask;
    public GameObject thisEnemiesGun;

    public float maxSeeingDistance;

    public float rotationSpeedMultiplier;

    [Header("Tempor�ra testvarialber")]
    public GameObject ammo;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (true) //om spelaren �r inne i en viss zon
        {
            FaceTarget();
            if (true) //om spelaren �r tillr�ckligt n�ra
            {
                StartCoroutine(EnemyFireTest());
                EnemyFireTest();
            }
        }
    }

    IEnumerator EnemyFireTest()
    {
        Debug.Log("Enemy reloading...");
        yield return new WaitForSeconds(2f);
        Debug.Log("Enemy firing at player");
        Instantiate(ammo, thisEnemiesGun.transform.position, Quaternion.identity);
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeedMultiplier);
        //g�r s� att fienden roterar mot spelaren
    }
}
