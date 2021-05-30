using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Text loadingPercent;

    public IEnumerator AsynchronousLoad(string scene)
    {
        yield return null;

        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            int currentProgress = (int)operation.progress;

            loadingPercent.text = currentProgress.ToString() + " %";

            if(operation.isDone)
            {
                operation.allowSceneActivation = true;
            }

        }
        yield return null;
    }
}
