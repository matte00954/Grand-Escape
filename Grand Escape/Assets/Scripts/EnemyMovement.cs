using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Gameobjects")]
    public GameObject player;
    public LayerMask playerMask;

    public float maxSeeingDistance;

    public float rotationSpeedMultiplier;

    // Update is called once per frame
    void Update()
    {
        FaceTarget();
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeedMultiplier);
        //g�r s� att fienden roterar mot spelaren
        //OBS verkar f�r spelet att lagga? finns det en b�ttre l�sning?
    }
}