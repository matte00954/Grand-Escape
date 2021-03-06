using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [SerializeField] private float cooldown = 2f;
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
}
