//Main author: Mattias Larsson

using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoadData : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public void SavePlayer()
    {
        Debug.Log("Saving player data");
        SaveSystem.SavePlayer(player.GetComponent<PlayerVariables>(),SceneManager.GetActiveScene().name);
    }

    public void LoadPlayer()
    {
        Debug.Log("Loading player data");
        PlayerData data = SaveSystem.LoadPlayer();

        /*
        data.level;

        data.savedHealthPoints;

        data.savedAmmoCount;

        data.savedSelectedWeapon;

        data.savedStaminaPoints;
        */

        Vector3 position;
        position.x = data.position[0];
        position.y = data.position[1];
        position.z = data.position[2];

        player.GetComponent<PlayerMovement>().TeleportPlayer(position); //temp, should happen at start not here
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SavePlayer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            LoadPlayer();
        }

    }
}

/*
public string level;

public int savedHealthPoints;
public int savedAmmoCount;
public int savedSelectedWeapon;

public float savedStaminaPoints;
public float[] position; //Can not save vector3 here, instead will save 3 floats, x y z
*/

//https://www.youtube.com/watch?v=XOjd_qU2Ido&t=548s