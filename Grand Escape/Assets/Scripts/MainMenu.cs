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
    private bool settingsOpen;


    void Start()
    {
        settingsMenu.SetActive(false);
        settingsOpen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //buttonAnim.SetTrigger("pressed");
            if (!settingsOpen)
            {
                OpenSettings();
            }
            else if (settingsOpen)
            {
                CloseSettings();
            }
        }

        if (settingsOpen)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CloseSettings();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                ReturnToMain();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                QuitGame();
            }
        }
    }

    public bool isPaused()
    {
        return settingsOpen;
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
        Time.timeScale = 0.01f;
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        settingsOpen = false;
        Time.timeScale = 1;
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        //exitAnim.SetTrigger("fadeOut");
        Debug.Log("Game was Quit");
        Application.Quit();
    }
    IEnumerator Delay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(1);
    }
}
