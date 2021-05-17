//Main author: Mattias Larsson
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private string sceneToSwitchTo;
    [SerializeField] private string[] sceneList;

    public void ChangeScene()
    {
        Debug.Log("Changing to scene: " + sceneToSwitchTo);
        SceneManager.LoadScene(sceneToSwitchTo);
    }
}
