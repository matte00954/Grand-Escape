using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EquippedSword : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;

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

    private void Swoosh()
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        audioSource.PlayOneShot(clip);
    }
}
