using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    //TODO Get whole list here in a array?
    [SerializeField] string sceneToSwitchTo;

    [SerializeField] string[] sceneList;

    public void ChangeScene()
    {
        Debug.Log("Scene end reached");
        SceneManager.LoadScene(sceneToSwitchTo);
    }
}
