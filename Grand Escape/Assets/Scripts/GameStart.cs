//Main Author: Miranda Greting
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStart : MonoBehaviour
{
    [SerializeField] private GameObject startText;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Animator newGameAnim;
    [SerializeField] private Image loadingScreen;

    [SerializeField] private string newGameFirstLevel = "Level 1 Main";
    [SerializeField] private float delayTime = 1f;
    private Animator anim;
    private Animator menuAnim;

    // Start is called before the first frame update
    void Start()
    {
        LoadScreenEnabled(false);
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

    public void LoadSavedGame()
    {
        LoadHandler.isSavedGame = true;
        PlayerData data = SaveSystem.LoadPlayer();
        LoadScreenEnabled(true);
        LoadScene(data.sceneName);
    }

    private void LoadScene(int sceneIndex) => SceneManager.LoadSceneAsync(sceneIndex);
    private void LoadScene(string sceneName) => SceneManager.LoadSceneAsync(sceneName);

    public void OpenSettings() => settingsMenu.SetActive(true);
    public void CloseSettings() => settingsMenu.SetActive(false);

    public void NewGame()
    {
        LoadHandler.isSavedGame = false;
        StartCoroutine(StartDelay(delayTime));
        newGameAnim.SetTrigger("fadeOut");
        LoadScreenEnabled(true);
        LoadScene(newGameFirstLevel); 
    }

    public void QuitGame()
    {
        //exitAnim.SetTrigger("fadeOut");
        Debug.Log("Game was Quit");
        Application.Quit();
    }

    private void LoadScreenEnabled(bool enabled)
    {
        loadingScreen.gameObject.SetActive(enabled);
    }

    IEnumerator StartDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

    IEnumerator DelayInactivation(float seconds)
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

}
