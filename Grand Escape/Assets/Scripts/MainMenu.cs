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
    public static bool IsPaused { get; private set; }

    void Start()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(false);
        howToPlay.SetActive(false);
        IsPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) //Pauses and unpauses the game with 'P'-key.
        {
            if (!IsPaused)
                OpenPause();
            else if (IsPaused)
                ClosePauseMenu();
            CloseHTP();
                CloseSettings();
        }

        if (IsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Return)) //Exits pausemenu
                ClosePauseMenu();
            if (Input.GetKeyDown(KeyCode.M)) //Returns the player to main/start menu
                ReturnToMain();
            if (Input.GetKeyDown(KeyCode.O)) //Quits the application
                QuitGame();
        }
    }

    public void OpenSettings() => settingsMenu.SetActive(true);
    public void CloseSettings() => settingsMenu.SetActive(false);
    public void OpenHTP() => howToPlay.SetActive(true); //Enables the 'how to play'-window.
    public void CloseHTP() => howToPlay.SetActive(false); //Disables the 'how to play'-window.

    private void OpenPause()
    {
        pauseMenu.SetActive(true);
        IsPaused = true;
        Time.timeScale = 0;
        UpdateCursorLockState();
    }   

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        IsPaused = false;
        Time.timeScale = 1;
        UpdateCursorLockState();
    }

    public void LoadSavedGame()
    {
        LoadHandler.isSavedGame = true;
        PlayerData data = SaveSystem.LoadPlayer();
        LoadScene(data.sceneName);
    }

    public void NewGame()
    {
        LoadHandler.isSavedGame = false;
    }

    private void LoadScene(int sceneIndex) => SceneManager.LoadSceneAsync(sceneIndex);

    private void LoadScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName);

    public void PressPause()    //method for pausebutton that toggles pause menu on or off depending on whether it's already opened
    {
        //buttonAnim.SetTrigger("pressed");
        if (!IsPaused)
            OpenPause();
        else if (IsPaused)
            ClosePauseMenu();
    }

    public void ReturnToMain() //returns player to start scene
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

    private IEnumerator Delay(int seconds) //method to delay code execution for a chosen number of second, e.g. for animation to finish playing
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(1);
    }

    private void UpdateCursorLockState()
    {
        if (IsPaused)
            Cursor.lockState = CursorLockMode.Confined;
        else
            Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = IsPaused;
    }
}
/*
 * //Main Author: Miranda Greting
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject startText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Animator newGameAnim;
    private Animator anim;
    private Animator menuAnim;

    // Start is called before the first frame update
    void Start()
    {
        anim = startText.GetComponent<Animator>();
        menuAnim = mainMenu.GetComponent<Animator>();
        //newGame = mainMenu.GetChild(0).gameObject;
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            anim.SetTrigger("fadeOut");
            StartCoroutine(DelayInactivation(1));
        }
    }

    public void OpenSettings() => settingsMenu.SetActive(true);
    public void CloseSettings() => settingsMenu.SetActive(false);

    public void StartGame()
    {
        newGameAnim.SetTrigger("fadeOut");
        StartCoroutine(StartDelay(1));
    }

    public void QuitGame()
    {
        //exitAnim.SetTrigger("fadeOut");
        Debug.Log("Game was Quit");
        Application.Quit();
    }

    IEnumerator StartDelay(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadScene(1);
    }

    IEnumerator DelayInactivation(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        startText.SetActive(false);
        mainMenu.SetActive(true);
        menuAnim.SetTrigger("fadeIn");
    }
    /*
    public void StartNewGame()
    {
        SceneManager.LoadScene(1);
    }
    */