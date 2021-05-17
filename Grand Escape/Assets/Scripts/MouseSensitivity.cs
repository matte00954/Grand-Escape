using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    [SerializeField] Slider sensitivitySlider;

    // Start is called before the first frame update
    void Start()
    {
        sensitivitySlider.onValueChanged.AddListener(delegate { SensitivityChanged(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SensitivityChanged()
    {
        //GetComponent<MouseLook>().ChangeMouseSensitivity(sensitivitySlider.value);
    }

    /*code needed in MouseLook: public void ChangeMouseSensitivity(float value) {
        mouseSensitivity = value;
    } */
        

}
