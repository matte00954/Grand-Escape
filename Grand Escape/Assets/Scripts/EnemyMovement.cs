//Author: William Örnquist
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [Header("Base variables")]
    [SerializeField] private LayerMask groundMask, playerMask, detectionMask;
    [SerializeField] private BoxCollider sightCollider;
    [SerializeField] private Transform headTransform;
    private GameObject player;
    private NavMeshAgent agent;
    private EnemyShooting enemyShooting;

    [Header("Patrol state")]
    [SerializeField] private bool IsStationary;
    [SerializeField] private float walkPointRange;
    [SerializeField] private float timeBetweenPatrol;
    [SerializeField] private float patrolSpeed;

    private Vector3 walkPoint;
    private bool walkPointSet;

    [Header("Detection")]
    [SerializeField] private float sightRange;
    [SerializeField] private float peripheralRange;
    [SerializeField] private float hearingRange;
    [SerializeField] private bool isDeaf;
    [SerializeField] private bool isBlind;

    [Header("Alert state")]
    [SerializeField] private float chasingSpeed;
    [SerializeField] private float maxAttackRange;
    [SerializeField] private float alertBufferTime;

    [Header("Attack state")]
    [SerializeField] private float rotationSpeed;
    
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
            if (agent.remainingDistance != Mathf.Infinity
            && agent.pathStatus == NavMeshPathStatus.PathComplete
            && agent.remainingDistance == 0)
                patrolTimer -= Time.deltaTime;

        UpdateDetectionRays();
        UpdateState();

        // Animations
        anim.SetFloat("Speed", agent.speed/20);
            
    }

    private void UpdateDetectionRays()
    {
        RaycastHit sightHit;
        RaycastHit peripheralHit;

        if (!isBlind && Physics.Raycast(headTransform.position, (player.transform.position - headTransform.position).normalized, out peripheralHit, peripheralRange, detectionMask))
            if (peripheralHit.collider.gameObject.tag == "Player")
                sawPlayer = true;

        if (!isBlind && Physics.Raycast(headTransform.position, (player.transform.position - headTransform.position).normalized, out sightHit, sightRange, detectionMask))
            if (sightHit.collider.gameObject.tag == "Player" && sightCollider.bounds.Contains(player.transform.position))
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
