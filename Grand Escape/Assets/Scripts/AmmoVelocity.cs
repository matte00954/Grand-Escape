using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoVelocity : MonoBehaviour
{
    //All other variables move to AmmoType Scriptable object

    [SerializeField] AmmoType ammo;

    private Vector3 direction;

    bool startTimerToRemoveBullet;

    // Start is called before the first frame update
    void Start()
    {
        direction = transform.forward;
    }

    private void Awake()
    {
        StartCoroutine(TimeUntilBulletGetsRemoved());
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * ammo.GetAmmoSpeed() * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) //Checka bara if sats om t.ex. bullet som ägs av fiende träffar en spelare.
    {
        Debug.Log("I hit: " + other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerVariables>().ApplyDamage(ammo.GetAmmoDamage());
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyVariables>().ApplyDamage(ammo.GetAmmoDamage());
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Ground")
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag == "Untagged") //TODO Temporary, might cause issues
        {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator TimeUntilBulletGetsRemoved() //IEnumerators might be an issue
    {
        yield return new WaitForSeconds(ammo.GetBulletDoesNotHitTimer());
        Destroy(this.gameObject);
    }
}
