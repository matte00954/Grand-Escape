using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    /*public int damage; //<---- TODO: inflict damage to player health */
    public float cooldown = 2f;
    private float timer;
    bool readyToHit;

    public PlayerVariables playerVariables;

    private void Awake()
    {
        playerVariables = GameObject.Find("First Person Player").GetComponent<PlayerVariables>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < cooldown)
            timer += Time.deltaTime;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 6 && timer >= cooldown)
        {
            Debug.Log("Melee hit on player");
            timer = 0f;
            playerVariables.ApplyDamage(50); 
        }
    }
}
