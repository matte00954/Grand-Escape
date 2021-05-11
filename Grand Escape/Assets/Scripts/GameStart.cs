using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    [SerializeField] GameObject startText;
    [SerializeField] GameObject mainMenu;
    private Animator anim;
    private Animator menuAnim;
    private GameObject newGame;

    // Start is called before the first frame update
    void Start()
    {
        anim = startText.GetComponent<Animator>();
        menuAnim = mainMenu.GetComponent<Animator>();
        //newGame = mainMenu.GetChild(0).gameObject;
        mainMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            anim.SetTrigger("fadeOut");
            StartCoroutine(DelayInactivation());
        }
    }

    IEnumerator DelayInactivation()
    {
        yield return new WaitForSeconds(1);
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
