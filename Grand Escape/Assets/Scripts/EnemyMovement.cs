//Author: William Örnquist
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Base variables")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private BoxCollider sightCollider;
    [SerializeField] private Transform headTransform;
    private GameObject player;
    private NavMeshAgent agent;
    private EnemyShooting enemyShooting;

    [Header("Patrol state")]
    [Tooltip("The enemy will never move."),
        SerializeField] private bool IsStationary;
    [Tooltip("The maximum distance from the next patrol point."),
        SerializeField] private float walkPointRadius;
    [Tooltip("Time between setting new destinations."),
        SerializeField] private float timeBetweenPatrol;
    [Tooltip("The maximum speed during patrol state."),
        SerializeField] private float patrolSpeed;

    private Vector3 walkPoint;
    private bool walkPointSet;

    [Header("Detection")]
    [Tooltip("The range of the enemy's line of sight detection of the player."),
        SerializeField] private float sightRange;
    [Tooltip("The range where the enemy will detect the player from any direction."),
        SerializeField] private float peripheralRange;
    [Tooltip("Not yet functional."),
        SerializeField] private float hearingRange;
    [Tooltip("Not yet functional."),
        SerializeField] private bool isDeaf;
    [Tooltip("Disables detection related to enemy's sight."), 
        SerializeField] private bool isBlind;

    [Header("Alert state")]
    [Tooltip("The maximum speed of the enemy in alerted state."), 
        SerializeField] private float chasingSpeed;
    [Tooltip("The maximum range from the player required for the enemy to stand still and attack."), 
        SerializeField] private float maxAttackRange;
    [Tooltip("The time it takes for the enemy to give up chasing after losing sight of the player."), 
        SerializeField] private float alertBufferTime;

    [Header("Attack state")]
    [Tooltip("The speed of enemy's rotation while standing still and attacking."), 
        SerializeField] private float rotationSpeed;
    
    private bool heardPlayer;
    private bool sawPlayer;
    private bool isAlerted;
    private bool isHurt;

    private float patrolTimer, alertTimer;

    // Animations
    private Animator anim;

    private void Start() => player = GameObject.Find("PlayerHead");

    private void Awake()
    {
        TryGetComponent(out enemyShooting);
        agent = GetComponent<NavMeshAgent>();
        patrolTimer = timeBetweenPatrol;

        // Animations
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (patrolTimer > 0f)
                patrolTimer -= Time.deltaTime;

        UpdateDetectionRays();
        UpdateState();

        // Animations
        anim.SetFloat("Speed", agent.velocity.magnitude);
        //Debug.LogWarning("Velocity: " + agent.velocity.magnitude); // I did my part
    }

    private void UpdateDetectionRays()
    {

        if (!isBlind && Physics.Raycast(headTransform.position, (player.transform.position - headTransform.position).normalized, out RaycastHit peripheralHit, peripheralRange, detectionMask))
            if (peripheralHit.collider.gameObject.CompareTag("Player"))
                sawPlayer = true;

        if (!isBlind && Physics.Raycast(headTransform.position, (player.transform.position - headTransform.position).normalized, out RaycastHit sightHit, sightRange, detectionMask))
            if (sightHit.collider.gameObject.CompareTag("Player") && sightCollider.bounds.Contains(player.transform.position))
                sawPlayer = true;
    }

    private void UpdateState()
    {
        if (!IsStationary && !heardPlayer && !sawPlayer && alertTimer <= 0f) //Patroling state when not detecting player and not in an alerted state
            Patroling();
        else if (heardPlayer || sawPlayer) //If enemy either sees or hears the player, it will reset and start the timer for alerted state
        {
            alertTimer = alertBufferTime;
            heardPlayer = false;
            sawPlayer = false;
            isAlerted = true;
            if (enemyShooting != null)
                enemyShooting.SetAlert(true);
        }

        if (alertTimer > 0f) //The enemy will chase the player's current position until 'alertTimer' hits 0.
        {
            if (IsStationary || Vector3.Distance(headTransform.position, player.transform.position) <= maxAttackRange)
                AttackPlayer();
            else
                ChasePlayer();

            alertTimer -= Time.deltaTime;
        }
        else if (isAlerted)
        {
            isAlerted = false;
            if (enemyShooting != null)
                enemyShooting.SetAlert(false);
        }
    }

    private void Patroling()
    {
        if (!walkPointSet && patrolTimer <= 0f)
        {
            SearchWalkPoint();
            patrolTimer = timeBetweenPatrol;
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
        //float randomZ = Random.Range(-walkPointRange, walkPointRange);
        //float randomX = Random.Range(-walkPointRange, walkPointRange);

        Vector3 randomDirection = Random.insideUnitSphere * walkPointRadius;
        randomDirection += transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkPointRadius, 1);
        walkPoint = hit.position;
        walkPointSet = true;

        //walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //RaycastHit raycastHit;

        //if (Physics.Raycast(walkPoint + Vector3.up * 5f, -transform.up, out raycastHit, 20f, groundMask))
        //{
        //    walkPoint.y = raycastHit.point.y;
        //    walkPointSet = true;
        //}
    }

    private void ChasePlayer()
    {
        agent.speed = chasingSpeed;
        agent.SetDestination(player.transform.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 direction = (player.transform.position - transform.position).normalized;
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
        if (player != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(headTransform.position, (player.transform.position - headTransform.position).normalized * sightRange);
            Gizmos.color = Color.green;
            Gizmos.DrawRay(headTransform.position, (player.transform.position - headTransform.position).normalized * peripheralRange);
        }
    }
}
