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

    private AudioSource source;

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

    [Tooltip("Aggrosounds"),
        SerializeField] private AudioClip [] alertSounds;

    [Header("Attack state")]
    [Tooltip("The speed of enemy's rotation while standing still and attacking."), 
        SerializeField] private float rotationSpeed;
    
    private bool heardPlayer;
    private bool sawPlayer;
    private bool isAlerted;
    private bool isHurt;

    private bool firstAttack;

    private float patrolTimer, alertTimer;
    private static float easeTimer;

    private Animator anim;

    private void Start() => player = GameObject.Find("PlayerHead");

    private void Awake()
    {
        firstAttack = true;

        TryGetComponent(out enemyShooting);
        agent = GetComponent<NavMeshAgent>();
        patrolTimer = timeBetweenPatrol;

        anim = GetComponent<Animator>();

        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (patrolTimer > 0f)
                patrolTimer -= Time.deltaTime;

        if (easeTimer > 0f)
            easeTimer -= Time.deltaTime;

        UpdateDetectionRays();
        UpdateState();

        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    /// <summary>
    /// Prevents all enemies from getting alerted for a set amount of time.
    /// </summary>
    /// <param name="timeInSeconds">Time before enemies are able to become alerted again.</param>
    public static void EaseAllEnemies(float timeInSeconds) => easeTimer = timeInSeconds;

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
        if (!IsStationary && !heardPlayer && !sawPlayer && alertTimer <= 0f || !PlayerVariables.isAlive || easeTimer > 0f) //Patroling state when not detecting player and not in an alerted state
        {
            heardPlayer = false;
            sawPlayer = false;
            alertTimer = 0f;
            Patroling();

            firstAttack = true;
        }
        else if (heardPlayer || sawPlayer) //If enemy either sees or hears the player, it will reset and start the timer for alerted state
        {

            if (firstAttack)
            {
                source.PlayOneShot(alertSounds[Random.Range(0, alertSounds.Length)]);
                firstAttack = false;
            }


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
        Vector3 randomDirection = Random.insideUnitSphere * walkPointRadius;
        randomDirection += transform.position;
        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, walkPointRadius, 1);
        walkPoint = hit.position;
        walkPointSet = true;
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
