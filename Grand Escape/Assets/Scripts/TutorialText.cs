//Author: Mattias Larsson
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private UiManager uiManager;
    [TextArea]
    [SerializeField] private string tutorialText;

    private void Start()
    {
        if(uiManager == null)
            Debug.LogError(this.gameObject + " this tutorial text needs a UiManager reference");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            uiManager.TutorialText(tutorialText, true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            uiManager.TutorialText("", false);
    }
}
