using UnityEngine;
using UnityEngine.AI;

public class EnemyMelee : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform playerTransform;
    public LayerMask groundMask, playerMask;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    float patrolTimer;
    public float timeBetweenPatrol;
    public float patrolSpeed;

    //Attacking
    public float sightRange;
    public float attackSpeed;
    public bool playerInSightRange;

    private void Awake()
    {
        playerTransform = GameObject.Find("First Person Player").transform;
        agent = GetComponent<NavMeshAgent>();
        patrolTimer = timeBetweenPatrol;
    }

    private void Update()
    {

        if (!(patrolTimer >= timeBetweenPatrol))
        {
            patrolTimer += Time.deltaTime;
        }

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, playerMask);

        if (!playerInSightRange)
            Patroling();
        if (playerInSightRange)
            ChasePlayer();
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
        agent.speed = attackSpeed;
        agent.SetDestination(playerTransform.position);
    }

    private void AttackPlayer()
    {

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
