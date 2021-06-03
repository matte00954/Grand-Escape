//Main Author: Miranda Greting
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject startText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Animator newGameAnim;
    [SerializeField] private GameObject loadingScreen;

    [SerializeField] private string newGameFirstLevel = "Level 1 Main";
    [SerializeField] private float delayTime = 1f;
    private Animator anim;
    private Animator menuAnim;

    private string functionToActivate;
    private float delayTimer;
    private bool delayIsActive;
    private bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
        LoadScreenEnabled(false);
        anim = startText.GetComponent<Animator>();
        menuAnim = mainMenu.GetComponent<Animator>();
        //newGame = mainMenu.GetChild(0).gameObject;
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            anim.SetTrigger("fadeOut");
            StartCoroutine(DelayInactivation(1));
        }

        if (delayTimer > 0f)
            delayTimer -= Time.unscaledDeltaTime;
        if (delayTimer <= 0f && delayIsActive)
        {
            delayIsActive = false;
            RunButtonFunction(functionToActivate);
        }
    }

    public void PressedDelayButton(string buttonName)
    {
        if (!delayIsActive)
        {
            delayIsActive = true;
            delayTimer = delayTime;
            functionToActivate = buttonName;

            switch (buttonName)
            {
                case "NewGame":
                    newGameAnim.SetTrigger("fadeOut");
                    break;
                default:
                    break;
            }
        }
    }

    public void RunButtonFunction(string name)
    {
        switch (name)
        {
            case "NewGame":
                NewGame();
                break;
            case "LoadGame":
                LoadSavedGame();
                break;
            default:
                Debug.LogError("Invalid button name.");
                break;
        }
    }

    public void LoadSavedGame()
    {
        LoadHandler.isSavedGame = true;
        PlayerData data = SaveSystem.LoadPlayer();
        LoadScreenEnabled(true);
        LoadScene(data.sceneName);
    }

    public void NewGame()
    {
        LoadHandler.isSavedGame = false;
        LoadScreenEnabled(true);
        LoadScene(newGameFirstLevel);
    }

    //private void LoadScene(int sceneIndex) => SceneManager.LoadSceneAsync(sceneIndex);
    private void LoadScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName);

    public void OpenSettings() => settingsMenu.SetActive(true);
    public void CloseSettings() => settingsMenu.SetActive(false);

    public void QuitGame()
    {
        //exitAnim.SetTrigger("fadeOut");
        Debug.Log("Game was Quit");
        Application.Quit();
    }

    private void LoadScreenEnabled(bool enabled)
    {
        loadingScreen.SetActive(enabled);
    }

    IEnumerator DelayInactivation(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        startText.SetActive(false);
        mainMenu.SetActive(true);
        menuAnim.SetTrigger("fadeIn");
    }

}
