using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//author Leo Mendonca Agild leme2980
public class PlayOneLiner : MonoBehaviour
{
    [SerializeField] private string[] soundNames;

    [SerializeField] private int minKills;
    [SerializeField] private int maxKills;

    [SerializeField] private float delay;


    private int killCount;
    private int randomNumber;
    // Start is called before the first frame update
    void Start()
    {
        randomNumber = Random.Range(minKills, maxKills);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(killCount);

        if (killCount==randomNumber)
        {
            killCount = 0;
            randomNumber = Random.Range(minKills, maxKills);
            Invoke("PlayLater", delay);

        }

    }

   private void PlayLater()
    {
        FindObjectOfType<AudioManager>().Play(soundNames[Random.Range(0, soundNames.Length+1)]);
    }

    public void SetKillCount(int amount)
    {
        killCount += amount;
    }


}
