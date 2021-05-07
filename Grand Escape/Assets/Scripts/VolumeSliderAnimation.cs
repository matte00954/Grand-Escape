using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderAnimation : MonoBehaviour
{
    [SerializeField] Sprite[] handleImages;
    [SerializeField] GameObject handle;
    [SerializeField] Text volumeText;
    private Slider slider;
    private Sprite handleImage;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        handleImage = handle.GetComponent<Image>().sprite;
        volumeText.text = "100";
    }

    // Update is called once per frame
    void Update()
    {
        int volume = (int)slider.value;
        volumeText.text = volume.ToString();
        if (slider.value <= 100 && slider.value >= 90)
        {
            Debug.Log("WorksSoFar");
            handle.GetComponent<Image>().sprite = handleImages[0];
        }
        else if (slider.value <= 90 && slider.value >= 80)
        {
            handle.GetComponent<Image>().sprite = handleImages[1];
        }
        else if (slider.value <= 80 && slider.value >= 70)
        {
            handle.GetComponent<Image>().sprite = handleImages[2];
        }
        else if (slider.value <= 70 && slider.value >= 60)
        {
            handle.GetComponent<Image>().sprite = handleImages[3];
        }
        else if (slider.value <= 60 && slider.value >= 50)
        {
            handle.GetComponent<Image>().sprite = handleImages[4];
        }
        else if (slider.value <= 60 && slider.value >= 50)
        {
            handle.GetComponent<Image>().sprite = handleImages[4];
        }
        else if (slider.value <= 50 && slider.value >= 40)
        {
            handle.GetComponent<Image>().sprite = handleImages[5];
        }
        else if (slider.value <= 40 && slider.value >= 30)
        {
            handle.GetComponent<Image>().sprite = handleImages[6];
        }
        else if (slider.value <= 30 && slider.value >= 20)
        {
            handle.GetComponent<Image>().sprite = handleImages[7];
        }
        else if (slider.value <= 20 && slider.value >= 10)
        {
            handle.GetComponent<Image>().sprite = handleImages[8];
        }
        else if (slider.value <= 10 && slider.value >= 0)
        {
            handle.GetComponent<Image>().sprite = handleImages[9];
        }
        //note to self: gör om till en switch-sats
    }
}
