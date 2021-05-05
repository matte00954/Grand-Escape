using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] Animator newGameAnim;
    [SerializeField] Animator exitAnim;
    [SerializeField] Animator buttonAnim;
    private bool settingsOpen = false;


    void Start()
    {
        settingsMenu.SetActive(false);
    }

    public void StartGame()
    {
        newGameAnim.SetTrigger("fadeOut");
        StartCoroutine(Delay(1));
    }

    public void PressSettings()
    {
        buttonAnim.SetTrigger("pressed");
        if (!settingsOpen)
        {
            OpenSettings();
        }
        else if (settingsOpen)
        {
            CloseSettings();
        }
    }

    private void OpenSettings()
    {
        settingsMenu.SetActive(true);
        settingsOpen = true;
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        settingsOpen = false;
    }

    public void QuitGame()
    {
        exitAnim.SetTrigger("fadeOut");
        Application.Quit();
    }
    IEnumerator Delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(1);
    }
}
