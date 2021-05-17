using UnityEngine;
using UnityEngine.Events;

public class EquippedSword : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private BoxCollider swordCollider;
    [SerializeField] private UnityEvent OnAttack;

    [SerializeField] private UiManager uiManager;

    private AudioSource audioSource;
    private Animator anim;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();

        if(uiManager == null)
        {
            Debug.LogError("uiManager not set on EquippedSword");
        }

    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("ES_Slash"))
        {
            OnAttack.Invoke();
        }
    }

    private void OnDisable() => swordCollider.enabled = false;

    private void OnEnable()
    {
        uiManager.WeaponStatus(false);
        swordCollider.enabled = false;
    }

    private void AttackStart()
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
        swordCollider.enabled = true;
    }

    private void AttackEnd() => swordCollider.enabled = false;
}
