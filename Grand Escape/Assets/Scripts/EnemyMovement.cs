using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyMovement : MonoBehaviour
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

    [Header("Alert state")]
    [SerializeField] float chasingSpeed;
    [SerializeField] float maxAttackRange;
    [SerializeField] float alertBufferTime;

    [Header("Alert events")]
    [SerializeField] UnityEvent OnAlertStart;
    [SerializeField] UnityEvent OnAlertEnd;

    [Header("Attack state")]
    [SerializeField] float rotationSpeed;
    
    bool heardPlayer;
    bool sawPlayer;
    bool isAlerted;
    bool isHurt;

    float patrolTimer, alertTimer;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        patrolTimer = timeBetweenPatrol;
    }

    private void Update()
    {
        playerTransform = FindObjectOfType<PlayerMovement>().transform;

        if (!(patrolTimer >= timeBetweenPatrol))
            patrolTimer += Time.deltaTime;

        UpdateDetectionRays();

        if (!IsStationary && !heardPlayer && !sawPlayer && alertTimer <= 0f) //Patroling state when not detecting player and not in an alerted state
            Patroling();
        else if (heardPlayer || sawPlayer) //If enemy either sees or hears the player, it will reset and start the timer for alerted state
        {
            alertTimer = alertBufferTime;
            heardPlayer = false;
            sawPlayer = false;
            isAlerted = true;
            OnAlertStart.Invoke();
        }

        if (alertTimer > 0f) //The enemy will chase the player's current position until 'alertTimer' hits 0.
        {
            if (IsStationary || Vector3.Distance(transform.position, playerTransform.position) <= maxAttackRange)
                AttackPlayer();
            else
                ChasePlayer();

            alertTimer -= Time.deltaTime;
        }
        else if (isAlerted)
        {
            isAlerted = false;
            OnAlertEnd.Invoke();
        }
            
    }

    private void UpdateDetectionRays()
    {
        RaycastHit sightHit;
        RaycastHit peripheralHit;

        if (!isBlind && Physics.Raycast(transform.position, (playerTransform.position - transform.position).normalized, out peripheralHit, peripheralRange, detectionMask))
            if (peripheralHit.collider.gameObject.tag == "Player")
                sawPlayer = true;

        if (!isBlind && Physics.Raycast(transform.position, (playerTransform.position - transform.position).normalized, out sightHit, sightRange, detectionMask))
            if (sightHit.collider.gameObject.tag == "Player" && sightCollider.bounds.Contains(playerTransform.position))
                sawPlayer = true;
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
        agent.speed = chasingSpeed;
        agent.SetDestination(playerTransform.position);
    }

    private void Scan()
    {
        //Make stationary enemies look around.
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 direction = (playerTransform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
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
        if (playerTransform != null)
        {
            Gizmos.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * sightRange);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(transform.position, (playerTransform.position - transform.position).normalized * peripheralRange);
        }
    }
}
