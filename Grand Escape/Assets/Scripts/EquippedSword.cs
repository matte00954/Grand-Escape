using UnityEngine;
using UnityEngine.Events;

public class EquippedSword : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private BoxCollider swordCollider;
    [SerializeField] private UnityEvent OnAttack;

    private AudioSource audioSource;
    private Animator anim;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("ES_Slash"))
        {
            OnAttack.Invoke();
        }
    }

    private void OnDisable() => swordCollider.enabled = false;
    private void OnEnable() => swordCollider.enabled = false;

    private void AttackStart()
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
        swordCollider.enabled = true;
    }

    private void AttackEnd() => swordCollider.enabled = false;
}
