using UnityEngine;

public class Killzone : MonoBehaviour
{
    private readonly int PLAYER_DEATH_DAMAGE = 101;
    private readonly string PLAYER_TAG_NAME = "Player";

    private PlayerVariables playerVariables;

    private void Start() => playerVariables = FindObjectOfType<PlayerVariables>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == PLAYER_TAG_NAME)
            playerVariables.ApplyDamage(PLAYER_DEATH_DAMAGE);
    }
}
