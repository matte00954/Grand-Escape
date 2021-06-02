//Main author: Mattias Larsson
//Secondary author: William �rnquist
//this script should be placed on the game manager
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoadData : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if (LoadHandler.sceneChanged)
        {
            Load(false);
            LoadHandler.sceneChanged = false;
        }

        if (LoadHandler.isSavedGame)
        {
            Load(true);
            LoadHandler.isSavedGame = false;
        }
        else if (SceneManager.GetActiveScene().name != "MainMenu")
            Save();
    }

    public void Save() //from checkpoint
    {
        Debug.Log("Saving player data");
        SaveSystem.SavePlayer(FindObjectOfType<PlayerVariables>(),GetComponent<CheckpointRespawnHandler>(), SceneManager.GetActiveScene().name);
    }

    private void Load(bool mainMenuLoad) //true == main menu load, false == scene load
    {
        Debug.Log("Loading player data");
        PlayerData data = SaveSystem.LoadPlayer();

        FindObjectOfType<PlayerVariables>().SetStatsAfterSaveLoad(data.savedHealthPoints, data.savedAmmoCount, data.savedStaminaPoints, data.savedCheckPoint);

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

        if (mainMenuLoad)
        {
            Vector3 position;
            position.x = data.respawnPosition[0];
            position.y = data.respawnPosition[1];
            position.z = data.respawnPosition[2];

            Quaternion rotation = Quaternion.identity;
            rotation.x = data.respawnRotation[0];
            rotation.y = data.respawnRotation[1];
            rotation.z = data.respawnRotation[2];

            player.GetComponent<PlayerMovement>().TeleportPlayer(position, rotation);

            GetComponent<CheckpointRespawnHandler>().DeactivateEnemies(data.savedCheckPoint);

        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) //TESTING
        {
            Debug.Log("Static bool isSavedGame = " + LoadHandler.isSavedGame);
            Debug.Log("Static bool sceneChanged = " + LoadHandler.sceneChanged);
        }
    }
}

//https://www.youtube.com/watch?v=XOjd_qU2Ido&t=548s