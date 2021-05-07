using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimation : MonoBehaviour
{
    //script to be attached to a Toggle gameobject to animate its active and inactive states
    
    [SerializeField] GameObject sword; //Image to be activated/deactivated on ToggleValueChanged
    [SerializeField] Animator swordAnim;
    private Toggle toggle;


    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();    //locates the toggle component on the gameobject this script is attached to
        toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(toggle);
        }); //adds a listener to when toggle value is changed
        sword.SetActive(false);     //deactivates sword image 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ToggleValueChanged(Toggle change)
    {
        if (!toggle.isOn)
        {
            sword.SetActive(true);
            swordAnim.SetTrigger("fadeOut");    
        }
        else if(toggle.isOn)
        {
            sword.SetActive(false);
            swordAnim.SetTrigger("fadeIn");
        }
    }
}
