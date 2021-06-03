using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//author Leo Mendonda Agild leme2980
public class ActivateCollider : MonoBehaviour
{
    [SerializeField] private EnemyVariables enemy;
    [SerializeField] private BoxCollider boxCollider;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(enemy.activeInHierarchy);

        if (!enemy.enabled)
        {
            //Debug.Log("DEAD");
            GetComponent<BoxCollider>().enabled = true;
            boxCollider.enabled = true;
        }
    }
}
