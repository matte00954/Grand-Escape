using UnityEngine;
using UnityEngine.UI;

public class ToggleAnimation : MonoBehaviour
{
    //script to be attached to a Toggle gameobject to animate its active and inactive states
    
    [SerializeField] private Image sword; //Image to be activated/deactivated on ToggleValueChanged
    //[SerializeField] Animator swordAnim;
    [SerializeField] private Image inactiveSword;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider musicVolume;
    private Toggle toggle;
    private VolumeSliderAnimation masterScript;
    private VolumeSliderAnimation musicScript;
    private int lastMasterValue = 100;
    private int lastMusicValue = 100;


    // Start is called before the first frame update
    private void Start()
    {
        toggle = GetComponent<Toggle>();    //locates the toggle component on the gameobject this script is attached to
        toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(toggle);
        }); //adds a listener to when toggle value is changed
        inactiveSword = inactiveSword.GetComponent<Image>();
        inactiveSword.color = new Color(inactiveSword.color.r, inactiveSword.color.g, inactiveSword.color.b, 0f);     //deactivates sword image
        sword = sword.GetComponent<Image>();
        masterVolume.onValueChanged.AddListener(delegate { MasterValueChanged(); });
        musicVolume.onValueChanged.AddListener(delegate { MusicValueChanged(); });
        masterScript = masterVolume.GetComponent<VolumeSliderAnimation>();
        musicScript = musicVolume.GetComponent<VolumeSliderAnimation>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(masterVolume.value == 0 && musicVolume.value == 0)
        {
            toggle.isOn = false;
        }
    }

    private void MasterValueChanged()
    {
        if (!toggle.isOn)
        {
            //toggle.isOn = true;
        }
        if(masterVolume.value != 0)
        {
            lastMasterValue = (int)masterVolume.value;
            toggle.isOn = true;
        }
    }

    private void MusicValueChanged()
    {
        if (!toggle.isOn)
        {
            //toggle.isOn = true;
        }
        if (musicVolume.value != 0)
        {
            lastMusicValue = (int)musicVolume.value;
            toggle.isOn = true;
        }
    }

    private void ToggleValueChanged(Toggle change)
    {
        if (!toggle.isOn)
        {
            sword.color = new Color(sword.color.r, sword.color.g, sword.color.b, 0.5f);
            inactiveSword.color = new Color(inactiveSword.color.r, inactiveSword.color.g, inactiveSword.color.b, 0.5f);
            masterVolume.value = 0;
            musicVolume.value = 0;
            //swordAnim.SetTrigger("fadeOut");    
        }

        else if(toggle.isOn)
        {
            sword.color = new Color(sword.color.r, sword.color.g, sword.color.b, 1f);
            inactiveSword.color = new Color(inactiveSword.color.r, inactiveSword.color.g, inactiveSword.color.b, 0f);
            masterVolume.value = lastMasterValue;
            musicVolume.value = lastMusicValue;
            //swordAnim.SetTrigger("fadeIn");
        }
    }

    public void SwitchOn()
    {
        toggle.isOn = true;
    }
}
