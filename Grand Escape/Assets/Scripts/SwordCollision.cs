using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    [SerializeField] float damageOnHit;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GetComponent<BoxCollider>().enabled = false;
            other.gameObject.GetComponent<EnemyVariables>().ApplyDamage(damageOnHit);
        }
    }
}
