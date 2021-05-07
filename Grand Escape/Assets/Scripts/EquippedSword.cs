using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquippedSword : MonoBehaviour
{
    [SerializeField] AudioClip[] clips;
    [SerializeField] BoxCollider swordCollider;

    [SerializeField] UnityEvent OnAttack;

    private AudioSource audioSource;
    private Animator anim;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    void Update()
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
