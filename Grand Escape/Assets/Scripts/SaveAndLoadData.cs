//Main author: Mattias Larsson
//this script should be placed on the game manager
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoadData : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Start()
    {
        if (player.GetComponent<PlayerVariables>().GetCurrentCheckPoint() > 0)
        {
            LoadPlayerFromMainMenu();
        }
        else
            LoadPlayerOnNextLevel();
    }

    public void SavePlayerFromMainMenu() //should be called from UI
    {
        Debug.Log("Saving player data");
        SaveSystem.SavePlayer(player.GetComponent<PlayerVariables>(),SceneManager.GetActiveScene().name);
    }


    public void SavePlayerOnSceneChange() //should be called from scene
    {
        Debug.Log("Saving player data");
        SaveSystem.SavePlayer(player.GetComponent<PlayerVariables>(), "");
    }

    public void LoadPlayerFromMainMenu() //should be called from UI
    {
        Debug.Log("Loading player data from main menu");
        PlayerData data = SaveSystem.LoadPlayer();

        player.GetComponent<PlayerVariables>().SetStatsAfterSaveLoad(data.savedHealthPoints, data.savedAmmoCount, data.savedStaminaPoints, data.savedCheckPoint);

        Vector3 position;
        position.x = data.respawnPosition[0];
        position.y = data.respawnPosition[1];
        position.z = data.respawnPosition[2];

        player.GetComponent<PlayerMovement>().TeleportPlayer(position);

        if (data.pistolUnlocked)
        {
            WeaponHolder.UnlockWeaponSlot(0);
        }
        if (data.musketUnlocked)
        {
            WeaponHolder.UnlockWeaponSlot(1);
        }
        if (data.swordUnlocked)
        {
            WeaponHolder.UnlockWeaponSlot(2);
        }

        GetComponent<CheckpointRespawnHandler>().DeactivateEnemies(data.savedCheckPoint);
    }

    public void LoadPlayerOnNextLevel()
    {
        Debug.Log("Loading player data on next level");
        PlayerData data = SaveSystem.LoadPlayer();

        player.GetComponent<PlayerVariables>().SetStatsAfterSaveLoad(data.savedHealthPoints, data.savedAmmoCount, data.savedStaminaPoints, 0); //0 (last parameter) means that the next level should start at checkpoint 0

        if (data.pistolUnlocked) //all weapons should be unlocked at this point
        {
            WeaponHolder.UnlockWeaponSlot(0);
        }
        if (data.musketUnlocked)
        {
            WeaponHolder.UnlockWeaponSlot(1);
        }
        if (data.swordUnlocked)
        {
            WeaponHolder.UnlockWeaponSlot(2);
        }

        SaveSystem.SavePlayer(player.GetComponent<PlayerVariables>(), SceneManager.GetActiveScene().name); //need to save again here to make sure that the correct scene reference gets saved on file

    }

    private void Update()
    {
        ///
        /// Update method here only for testing, need to remove before final version!!!
        ///

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SavePlayerFromMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            LoadPlayerFromMainMenu();
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        ///
        /// Update method here only for testing, need to remove before final version!!!
        ///
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