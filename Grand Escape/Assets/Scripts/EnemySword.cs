using UnityEngine;

public class EnemySword : MonoBehaviour
{
    [SerializeField] private BoxCollider swordCollider;
    [SerializeField] private AudioClip[] swingClips;
    [SerializeField] private AudioClip[] hitClips;

    [SerializeField, Min(0)] private int damage = 40;

    private PlayerVariables playerVariables;
    private int clipIndex;

    private void Awake() => playerVariables = FindObjectOfType<PlayerVariables>();

    private void OnTriggerEnter(Collider other)
    {
        if (swordCollider == enabled && other.gameObject.CompareTag("Player"))
        {
            playerVariables.ApplyDamage(damage);
            clipIndex = Random.Range(1, hitClips.Length);
            AudioClip clip = hitClips[clipIndex];
            AudioSource.PlayClipAtPoint(clip, transform.position);
            hitClips[clipIndex] = hitClips[0];
        }
    }

    public void AttackStart() //Is called in animation event.
    {
        swordCollider.enabled = true;

        clipIndex = Random.Range(1, swingClips.Length);
        AudioClip clip = swingClips[clipIndex];
        AudioSource.PlayClipAtPoint(clip, transform.position);
        swingClips[clipIndex] = swingClips[0];
    }

    //Is called in animation event.
    public void AttackEnd() => swordCollider.enabled = false;
}
