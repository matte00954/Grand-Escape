//Author: William Örnquist
using UnityEngine;

public class SwordCollision : MonoBehaviour
{
    [SerializeField] private float damageOnHit;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            GetComponent<BoxCollider>().enabled = false;
            other.gameObject.GetComponent<EnemyVariables>().ApplyDamage(damageOnHit);
        }
    }
}
