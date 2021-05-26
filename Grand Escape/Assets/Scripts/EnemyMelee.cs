using UnityEngine;
using UnityEngine.Events;

public class EnemyMelee : MonoBehaviour
{
    [Tooltip("The time between attacks in seconds."),
        SerializeField] private float cooldown = 2f;
    [SerializeField] EnemySword enemySword;
    private float timer;
    private Animator anim;
    
    private void Awake() => anim = GetComponent<Animator>();

    void Update()
    {
        if (timer > 0f)
            timer -= Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && timer <= 0f)
        {
            Debug.Log("Melee hit on player");
            timer = cooldown;

            anim.SetTrigger("Attack");
        }
    }

    private void AttackStart() => enemySword.AttackStart();
    private void AttackEnd() => enemySword.AttackEnd();
}
