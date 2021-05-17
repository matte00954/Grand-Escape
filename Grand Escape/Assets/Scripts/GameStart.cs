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

}
