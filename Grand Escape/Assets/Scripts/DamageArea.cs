using UnityEngine;
using UnityEngine.Events;

public class DamageArea : MonoBehaviour
{
    /*public int damage; //<---- TODO: inflict damage to player health */
    [SerializeField] private PlayerVariables playerVariables;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private int damage = 40;

    private float timer;
    private bool readyToHit;

    // Animations
    [SerializeField] private AudioClip[] swingClips;
    [SerializeField] private AudioClip[] hitClips;
    [SerializeField] private BoxCollider swordCollider;
    [SerializeField] private UnityEvent onAttack;

    private AudioSource audioSource;
    private Animator anim;
    private int clipIndex;

    private void Awake()
    {
        playerVariables = GameObject.Find("First Person Player").GetComponent<PlayerVariables>();

        // Animations
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < cooldown)
            timer += Time.deltaTime;
    }

    // Animations
    private void AttackStart()
    {
        swordCollider.enabled = true;

        clipIndex = Random.Range(1, swingClips.Length);
        AudioClip clip = swingClips[clipIndex];
        AudioSource.PlayClipAtPoint(clip, transform.position);
        swingClips[clipIndex] = swingClips[0];
    }

    // Animations
    private void AttackEnd() => swordCollider.enabled = false;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6 && timer >= cooldown)
        {
            Debug.Log("Melee hit on player");
            timer = 0f;
            //playerVariables.ApplyDamage(damage);  <-- Moved downwards into if-statement

            // Animations
            onAttack.Invoke();

            // Animations
            if (swordCollider == enabled)
            {
                playerVariables.ApplyDamage(damage);
                clipIndex = Random.Range(1, hitClips.Length);
                AudioClip clip = hitClips[clipIndex];
                AudioSource.PlayClipAtPoint(clip, transform.position);
                hitClips[clipIndex] = hitClips[0];
            }

        }
    }
}
