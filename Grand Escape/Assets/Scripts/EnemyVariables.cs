//Main author: Mattias Larsson
//Secondary author: William Örnquist
using UnityEngine;
using UnityEngine.AI;

public class EnemyVariables : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private ParticleSystem deathParticleEffect;
    [SerializeField] private AudioClip[] damagedClips;
    [SerializeField] private AudioClip[] deathClips;

    [HideInInspector] public bool isAlive = true;


    [SerializeField] private GameObject healthPickupDrop;
    [SerializeField] private GameObject ammoPickupDrop;

    private AudioSource audioSource;
    private Animator anim;
    private Vector3 startPosition;
    private int healthPoints;

    private void Start()
    {
        ResetAllStats();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;
        anim = GetComponent<Animator>();
        anim.SetLayerWeight(1, 1);

        // Animations
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(healthPoints <= 0 && isAlive)
        {
            Debug.Log("Enemy dies");
            anim.SetLayerWeight(1, 0);
            // Animations
            anim.SetTrigger("Death");

            ParticleSystem particleSystem = Instantiate(deathParticleEffect);
            particleSystem.transform.position = transform.position;
            AudioClip clip = deathClips[Random.Range(0, deathClips.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);

            // När animationen Death har spelat klar ska fienden inaktiveras
            isAlive = false;
            Die();
        }
        anim.SetBool("IsAlive", isAlive);
    }

    private void Die()
    {
        FindObjectOfType<PlayOneLiner>().SetKillCount(1);

        PlayerVariables.AddStamina(enemyType.GetStaminaLeechAmount(), true);

        Vector3 pickupSpawnPosition;
        pickupSpawnPosition.x = this.gameObject.transform.position.x;
        pickupSpawnPosition.y = this.gameObject.transform.position.y + 1f; //to make sure pickup spawns above body
        pickupSpawnPosition.z = this.gameObject.transform.position.z;

        int dropTableIndex = Random.Range(0, 4);
        Debug.Log("dropTableIndex is " + dropTableIndex);
        if (dropTableIndex == 0)
            Instantiate(healthPickupDrop, pickupSpawnPosition, Quaternion.identity);
        else if (dropTableIndex == 1)
            Instantiate(ammoPickupDrop, pickupSpawnPosition, Quaternion.identity);
        else
            Debug.Log("Nothing was dropped");

        SetEnemyComponents(false);
    }

    private void SetEnemyComponents(bool enable)
    {
        foreach (MonoBehaviour component in GetComponents<MonoBehaviour>())
            component.enabled = enable;
        foreach (Collider col in GetComponentsInChildren<Collider>())
            col.enabled = enable;

        GetComponent<NavMeshAgent>().enabled = enable;
    }

    /// <summary>
    /// Applies a set amount of damage to enemy healthpool.
    /// </summary>
    /// <param name="damage">The amount of damage to apply.</param>
    public void ApplyDamage(float damage)
    {
        Debug.Log("Enemy took " + damage + " in damage");
        healthPoints -= (int)damage;

        // Animations
        if (healthPoints > 0)
        {
            // Animations
            anim.SetTrigger("Damaged");

            AudioClip clip = damagedClips[Random.Range(0, damagedClips.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }

    public void ResetAllStats()
    {
        healthPoints = enemyType.GetMaxHealthPoints();
        SetEnemyComponents(true);
        isAlive = true;
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}