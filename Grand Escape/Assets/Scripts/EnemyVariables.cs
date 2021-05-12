using UnityEngine;

public class EnemyVariables : MonoBehaviour
{
    [SerializeField] private EnemyType enemyType;
    [SerializeField] private ParticleSystem deathParticleEffect;
    [SerializeField] private AudioClip[] damagedClips;
    [SerializeField] private AudioClip[] deathClips;


    private int healthPoints;
    private AudioSource audioSource;

    private Vector3 startPosition;

    // Animations
    private Animator anim;

    private void Start()
    {
        ResetAllStats();
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;

        // Animations
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(healthPoints <= 0)
        {
            Debug.Log("Enemy dies");

            // Animations
            anim.SetTrigger("Death");

            ParticleSystem particleSystem = Instantiate(deathParticleEffect);
            particleSystem.transform.position = transform.position;
            AudioClip clip = deathClips[Random.Range(0, deathClips.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);

            // När animationen Death har spelat klar ska fienden inaktiveras
            this.gameObject.SetActive(false);
        }
    }

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
    }

    public void ResetPosition()
    {
        transform.position = startPosition;
    }
}