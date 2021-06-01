using UnityEngine;
using UnityEngine.UI;

public class MouseSensitivity : MonoBehaviour
{
    [SerializeField] Slider sensitivitySlider;

    void Start() => sensitivitySlider.onValueChanged.AddListener(delegate { SensitivityChanged(); });

    public void SensitivityChanged() => FindObjectOfType<MouseLook>().ChangeMouseSensitivity(sensitivitySlider.value);
}
