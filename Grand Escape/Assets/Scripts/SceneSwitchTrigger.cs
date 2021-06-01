//Main author: Mattias Larsson
using UnityEngine;

public class SceneSwitchTrigger : MonoBehaviour
{
    [Tooltip("Place game manager here")]
    [SerializeField] private SceneSwitch switchScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switchScript.ChangeScene();
        }
    }

}
