using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] LayerMask groundMask, playerMask, detectionMask;
    Transform playerTransform;
    [SerializeField] BoxCollider sightCollider;

    //Patroling
    [Header("Patrol state")]
    [SerializeField] bool IsStationary;
    [SerializeField] float walkPointRange;
    [SerializeField] float timeBetweenPatrol;
    [SerializeField] float patrolSpeed;

    Vector3 walkPoint;
    bool walkPointSet;

    [Header("Detection")]
    [SerializeField] float sightRange;
    [SerializeField] float peripheralRange;
    [SerializeField] float hearingRange;
    [SerializeField] bool isDeaf;
    [SerializeField] bool isBlind;

    //Attacking
    [Header("Chase state")]
    [SerializeField] float chasingSpeed;
    [SerializeField] float maxAttackRange;
    [SerializeField] float alertBufferTime;
    
    bool heardPlayer;
    bool seesPlayer;

    float patrolTimer, alertTimer = 0f;

    private void Awake()
    {
        //playerTransform = GameObject.Find("First Person Player").transform;
        agent = GetComponent<NavMeshAgent>();
        patrolTimer = timeBetweenPatrol;
    }

    private void Update()
    {
        if (!(patrolTimer >= timeBetweenPatrol))
            patrolTimer += Time.deltaTime;

        UpdateDetectionRays();

        if (!heardPlayer && !seesPlayer && alertTimer <= 0f) //Patroling state when not detecting player and not in an alerted state
            Patroling();
        else if (heardPlayer || seesPlayer) //If enemy either sees or hears the player, it will reset and start the timer for alerted state
        {
            alertTimer = alertBufferTime;
            heardPlayer = false;
            seesPlayer = false;
        }

        if (alertTimer > 0f) //The enemy will chase the player's current position until 'alertTimer' hits 0.
        {
            ChasePlayer();
            alertTimer -= Time.deltaTime;
        }
    }

    private void UpdateDetectionRays()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;

        RaycastHit sightHit;
        RaycastHit peripheralHit;

        if (!isBlind && Physics.Raycast(transform.position, (playerTransform.position - transform.position).normalized, out peripheralHit, peripheralRange, detectionMask))
            if (peripheralHit.collider.gameObject.tag == "Player")
            {
                Debug.Log("Peri detection");
                seesPlayer = true;
                return;
            }

        if (!isBlind && Physics.Raycast(transform.position, (playerTransform.position - transform.position).normalized, out sightHit, sightRange, detectionMask))
            if (sightHit.collider.gameObject.tag == "Player" && sightCollider.bounds.Contains(playerTransform.position))
            {
                Debug.Log("Sight detection");
                seesPlayer = true;
            }
                
    }

    private void Patroling()
    {
        if (!walkPointSet && patrolTimer >= timeBetweenPatrol)
        {
            SearchWalkPoint();
            patrolTimer = 0f;
        }

        if (walkPointSet)
        {
            agent.speed = patrolSpeed;
            agent.SetDestination(walkPoint);
            walkPointSet = false;
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        RaycastHit raycastHit;

        if (Physics.Raycast(walkPoint + Vector3.up * 5f, -transform.up, out raycastHit, 20f, groundMask))
        {
            walkPoint.y = raycastHit.point.y;
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        Vector3 distanceToPlayerPoint = transform.position - playerTransform.position;
        agent.speed = chasingSpeed;
        if (distanceToPlayerPoint.magnitude < 2f)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(playerTransform);
        }
        else
            agent.SetDestination(playerTransform.position);
    }

    public void ListenForPlayer()
    {
        Debug.LogWarning("LFP called");
        if (Physics.CheckSphere(transform.position, hearingRange, playerMask))
        {
            heardPlayer = true;
            Debug.Log("Enemy heard player");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, hearingRange);
        Gizmos.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * sightRange);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * peripheralRange);
    }
}
