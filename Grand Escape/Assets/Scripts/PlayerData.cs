//Main author: Mattias Larsson

[System.Serializable]
public class PlayerData
{
    public string sceneName;

    public int savedHealthPoints;
    public int savedAmmoCount;
    public int savedCheckPoint; //which checkpoint the player is currently at

    public float savedStaminaPoints;

    public bool pistolUnlocked;
    public bool musketUnlocked;
    public bool swordUnlocked;


    public float[] respawnPosition; //Can not save vector3 here, instead will save 3 floats, x y z, position refers to the respawn point

    public float[] respawnRotation; //To make sure player spawns with correct rotation

    public PlayerData(PlayerVariables playerVariables, CheckpointRespawnHandler checkpointRespawnHandler, string currentLevel)
    {

        sceneName = currentLevel;

        savedHealthPoints = playerVariables.GetCurrentHealthPoints();
        savedStaminaPoints = playerVariables.GetCurrentStamina();
        savedAmmoCount = playerVariables.GetCurrentAmmoReserve();
        savedCheckPoint = playerVariables.GetCurrentCheckPoint();

        pistolUnlocked = WeaponHolder.unlockedFlintPistol;
        musketUnlocked = WeaponHolder.unlockedMusket;
        swordUnlocked = WeaponHolder.unlockedSword;

        respawnPosition = new float[3]; 
        respawnPosition[0] = checkpointRespawnHandler.GetRespawnPoint().position.x;
        respawnPosition[1] = checkpointRespawnHandler.GetRespawnPoint().position.y;
        respawnPosition[2] = checkpointRespawnHandler.GetRespawnPoint().position.z;

        respawnRotation = new float[3];
        respawnPosition[0] = checkpointRespawnHandler.GetRespawnPoint().rotation.x;
        respawnPosition[1] = checkpointRespawnHandler.GetRespawnPoint().rotation.y;
        respawnPosition[2] = checkpointRespawnHandler.GetRespawnPoint().rotation.z;
    }
}
