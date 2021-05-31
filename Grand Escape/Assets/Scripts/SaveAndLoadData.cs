//Main author: Mattias Larsson
//this script should be placed on the game manager
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

        player.GetComponent<PlayerVariables>().SetStatsAfterSaveLoad(data.savedHealthPoints, data.savedAmmoCount, data.savedStaminaPoints, data.savedCheckPoint);

        Vector3 position;
        position.x = data.respawnPosition[0];
        position.y = data.respawnPosition[1];
        position.z = data.respawnPosition[2];

        player.GetComponent<PlayerMovement>().TeleportPlayer(position);

        GetComponent<CheckpointRespawnHandler>().DeactivateEnemies(data.savedCheckPoint);
    }

    private void Update()
    {
        ///
        /// Update method here only for testing
        ///

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SavePlayer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            LoadPlayer();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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