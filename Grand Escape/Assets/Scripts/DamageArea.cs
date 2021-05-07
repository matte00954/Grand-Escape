using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageArea : MonoBehaviour
{
    /*public int damage; //<---- TODO: inflict damage to player health */
    [SerializeField] PlayerVariables playerVariables;
    [SerializeField] float cooldown = 2f;
    [SerializeField] int damage = 40;

    float timer;
    bool readyToHit;

    // Animations
    [SerializeField] AudioClip[] swingClips;
    [SerializeField] AudioClip[] hitClips;
    [SerializeField] BoxCollider swordCollider;
    [SerializeField] UnityEvent onAttack;
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
