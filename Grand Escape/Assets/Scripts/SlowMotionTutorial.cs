using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionTutorial : MonoBehaviour
{

    [SerializeField] UiManager uiManager;

    [SerializeField] string tutorialText;

    private void Start()
    {
        if (uiManager == null)
        {
            Debug.LogError(this.gameObject + " this tutorial text needs a UiManager reference");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            uiManager.TutorialText(tutorialText, true);
        }
        else
        {
            uiManager.TutorialText("", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0.01f;
        }
    }
}
