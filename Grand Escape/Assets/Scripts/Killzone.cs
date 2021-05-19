//Author: William Örnquist
using UnityEngine;

public class Killzone : MonoBehaviour
{
    //private readonly int playerDeathDamage = 101;
    //private readonly string playerTagName = "Player";

    //private PlayerVariables playerVariables;

    //private void Start() => playerVariables = FindObjectOfType<PlayerVariables>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            FindObjectOfType<PlayerVariables>().ApplyDamage(101);
    }
}