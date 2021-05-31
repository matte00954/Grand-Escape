//Main author: Mattias Larsson
using UnityEngine;

public class SceneSwitchTrigger : MonoBehaviour
{
    [SerializeField] private SceneSwitch switchScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            switchScript.ChangeScene();
        }
    }

}
