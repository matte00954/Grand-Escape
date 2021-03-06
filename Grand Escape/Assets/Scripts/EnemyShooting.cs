//Main author: William ?rnquist
//Secondary author: Mattias Larsson
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject barrelEnd;
    [SerializeField] private GameObject enemyAmmo;
    [SerializeField] private ParticleSystem gunSmoke;
    [SerializeField] private EnemyVariables enemyVariables;
    private GameObject player;
    private Animator animator;

    [Header("Detection")]
    [SerializeField] private LayerMask aimCollisionMask;
    [SerializeField] private float maxFiringRange;

    [Header("Weapon")]
    [SerializeField] private float reloadTimeInSeconds;

    private readonly string fireName = "Fire";

    private float reloadTimer;
    private bool isAlerted = false;
    

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("PlayerHead");
    }

    private void Update()
    {
        if (enemyVariables.isAlive)
        {
            if (isAlerted)
            {
                RotateSelfToTarget();
                TakeAim();
                RotateGun();
            }
            else if (!isAlerted && barrelEnd.transform.rotation != Quaternion.identity)
                RotateGun();

            if (reloadTimer > 0f)
                reloadTimer -= Time.deltaTime;
        }
    }

    private void RotateGun()
    {
        if (isAlerted)
        {
            Vector3 direction = (player.transform.position - barrelEnd.transform.position).normalized;
            barrelEnd.transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            barrelEnd.transform.rotation = Quaternion.Euler(barrelEnd.transform.eulerAngles.x, barrelEnd.transform.eulerAngles.y, 0f);
        }
    }

    private void TakeAim()
    {
        if (isAlerted && Physics.Raycast(barrelEnd.transform.position, barrelEnd.transform.forward, out RaycastHit hit, maxFiringRange, aimCollisionMask) && reloadTimer <= 0f)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                reloadTimer = reloadTimeInSeconds;
                ShootWithGun();
            }
        }
    }

    private void RotateSelfToTarget()
    {
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime);
    }

    public void SetAlert(bool isAlerted) => this.isAlerted = isAlerted;

    private void ShootWithGun()
    {
        Instantiate(enemyAmmo, barrelEnd.transform.position, barrelEnd.transform.rotation);
        Instantiate(gunSmoke, barrelEnd.transform.position, barrelEnd.transform.rotation);
        // Animations
        animator.SetTrigger(fireName);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(barrelEnd.transform.position, barrelEnd.transform.forward * maxFiringRange);
    }
}
