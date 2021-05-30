//Author: Mattias Larsson
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private Image loadingScreen;

    //[SerializeField] private Text loadingPercent;

    [SerializeField] private string nextScene;

    public void ChangeScene()
    {
        loadingScreen.gameObject.SetActive(true);
        Debug.Log("Changing to scene: " + nextScene);
        SceneManager.LoadSceneAsync(nextScene);
        //SceneManager.LoadScene(sceneToSwitchTo);
        //StartCoroutine(AsynchronousLoad());
    }

    //private IEnumerator AsynchronousLoad()
    //{
    
    //    AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToSwitchTo);

    //    operation.allowSceneActivation = false;

    //    loadingScreen.gameObject.SetActive(true);

    //    while (!operation.isDone) 
    //    {
    //        float progress = Mathf.Clamp01(operation.progress / .9f); //progress goes from 0-0.9f, at 0.9f new scene can start loading

    //        Debug.Log("Loading: " + progress * 100f); 

    //        loadingPercent.text = progress * 100f + "%"; // * 100 because progress number would otherwise show 0.0 - 0.9

    //        if(operation.progress == 0.9f)
    //        {
    //            operation.allowSceneActivation = true;
    //        }
    //    }
    //    yield return null;
    //}
    //Code above does not work for some reason, might be a unity bug
}