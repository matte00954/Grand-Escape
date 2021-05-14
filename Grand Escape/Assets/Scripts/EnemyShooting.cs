//Main author: William Örnquist
//Secondary author: Mattias Larsson
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject barrelEnd;
    [SerializeField] private GameObject enemyAmmo;
    private GameObject player;

    [Header("Detection")]
    [SerializeField] private LayerMask aimCollisionMask;
    [SerializeField] private float maxFiringRange;

    [Header("Weapon")]
    [SerializeField] private float reloadTimeInSeconds;

    private float rotationSpeedMultiplier;
    private float reloadTimer;

    private bool isAlerted = false;

    // Animations
    private Animator animator;

    void Start()
    {
        // Animations
        animator = GetComponent<Animator>();
        player = GameObject.Find("First Person Player");
    }

    void Update()
    {
        if (isAlerted)
        {
            RotateSelfToTarget();
            TakeAim();
            RotateGun();
        }
        else if (!isAlerted && gun.transform.rotation != Quaternion.identity)
            RotateGun();

        if (reloadTimer > 0f)
            reloadTimer -= Time.deltaTime;
    }

    private void RotateGun()
    {
        if (isAlerted)
        {
            Debug.Log("Rotate gun to player: " + player.transform.position);
            Vector3 direction = (player.transform.position - barrelEnd.transform.position).normalized;
            barrelEnd.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            barrelEnd.transform.rotation = Quaternion.Euler(barrelEnd.transform.eulerAngles.x, barrelEnd.transform.eulerAngles.y, 0f);
        }
    }

    private void TakeAim()
    {
        RaycastHit hit;

        if (isAlerted && Physics.Raycast(barrelEnd.transform.position, barrelEnd.transform.forward, out hit, maxFiringRange, aimCollisionMask) && reloadTimer <= 0f)
        {
            Debug.Log("TakeAim hit something");
            if (hit.collider.gameObject.tag == "Player")
            {
                Debug.Log("TakeAim raycast hit player");
                reloadTimer = reloadTimeInSeconds;
                ShootWithGun();
            }
        }
    }

    private void RotateSelfToTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeedMultiplier);
    }

    public void SetAlert(bool isAlerted)
    {
        this.isAlerted = isAlerted;
    }

    private void ShootWithGun()
    {
        Instantiate(enemyAmmo, barrelEnd.transform.position, barrelEnd.transform.rotation);

        // Animations
        animator.SetTrigger("Fire");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(barrelEnd.transform.position, barrelEnd.transform.forward * maxFiringRange);
    }
}
