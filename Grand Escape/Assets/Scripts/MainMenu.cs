//Main Author: Miranda Greting
//Secondary Author: William Örnquist
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject howToPlay;
    //[SerializeField] private Animator newGameAnim;
    //[SerializeField] private Animator exitAnim;
    //[SerializeField] private Animator buttonAnim;

    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private PlayerShooting flintScript;
    [SerializeField] private PlayerShooting rifleScript;
    private bool paused;

    void Start()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        howToPlay.SetActive(false);
        paused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))   //pressing escape button toggles pause menu on or off depending on whether it's already active
        {
            if (!paused)
                OpenPause();
            else if (paused)
                ClosePause();
            CloseHTP();
                CloseSettings();
        }

        if (paused)     //checks if pausemenu is active
        {
            if (Input.GetKeyDown(KeyCode.Return))   //press return to exit/disable pausemenu
                ClosePause();
            if (Input.GetKeyDown(KeyCode.M))      //press m to return to main/start menu
                ReturnToMain();
            if (Input.GetKeyDown(KeyCode.O))        //press o to quit application
                QuitGame();
        }
    }

    public bool IsPaused()  //checks if game is paused (if pause menu is activated) or not
    {
        return paused;
    }

    public void OpenSettings() => settingsMenu.SetActive(true);
    public void CloseSettings() => settingsMenu.SetActive(false);
    public void OpenHTP() => howToPlay.SetActive(true);
    public void CloseHTP() => howToPlay.SetActive(false);

    private void OpenPause()    //activates pausemenu
    {
        pauseMenu.SetActive(true);
        paused = true;
        //Time.timeScale = 0.01f;
        Time.timeScale = 0;
        disableFPS();   //enables mouseclick
    }   

    public void ClosePause()    //inactivates pausemenu
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        paused = false;
        Time.timeScale = 1;
        disableFPS();
    }

    public void PressPause()    //method for pausebutton that toggles pause menu on or off depending on whether it's already opened
    {
        //buttonAnim.SetTrigger("pressed");
        if (!paused)
            OpenPause();
        else if (paused)
            ClosePause();
    }

    public void ReturnToMain()      //returns player to start scene
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
        //y not interactable? .~.
    }

    public void QuitGame()
    {
        Debug.Log("Game was Quit");
        Application.Quit();
    }

    private IEnumerator Delay(int seconds)  //method to delay code execution for a chosen number of second, e.g. for animation to finish playing
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(1);
    }

    private void disableFPS()
    {
        //implement in MouseLook and/or PlayerShooting script instead? Sätta en bool om att CursorLockMode.Locked inte ska va aktivt när pausmenyn är öppen?
        if (!paused)
        {
            //Enable mouselock + shooting script
            mouseLook.enabled = true;
            flintScript.enabled = true;
            rifleScript.enabled = true;
            Cursor.visible = false;
        }
        else
        {
            //Disable mouselock + shooting script
            mouseLook.enabled = false;
            flintScript.enabled = false;
            rifleScript.enabled = false;
            //Unlock Mouse and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
