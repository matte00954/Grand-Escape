//Main author: Mattias Larsson

[System.Serializable]
public class PlayerData
{
    public string level;

    public int savedHealthPoints;
    public int savedAmmoCount;
    public int savedCheckPoint; //which checkpoint the player is currently at

    public float savedStaminaPoints;

    public bool pistolUnlocked;
    public bool musketUnlocked;
    public bool swordUnlocked;


    public float[] respawnPosition; //Can not save vector3 here, instead will save 3 floats, x y z, position refers to the respawn point

    public PlayerData(PlayerVariables playerVariables, string currentLevel)
    {
        if(currentLevel.Equals("") || currentLevel.Equals(" "))
        {
            //may not use this
        }
        else
            level = currentLevel;

        savedHealthPoints = playerVariables.GetCurrentHealthPoints();
        savedStaminaPoints = playerVariables.GetCurrentStamina();
        savedAmmoCount = playerVariables.GetCurrentAmmoReserve();
        savedCheckPoint = playerVariables.GetCurrentCheckPoint();

        pistolUnlocked = WeaponHolder.unlockedFlintPistol;
        musketUnlocked = WeaponHolder.unlockedMusket;
        swordUnlocked = WeaponHolder.unlockedSword;

        respawnPosition = new float[3];

        respawnPosition[0] = playerVariables.GetCurrentRespawnPoint().position.x;
        respawnPosition[1] = playerVariables.GetCurrentRespawnPoint().position.y;
        respawnPosition[2] = playerVariables.GetCurrentRespawnPoint().position.z;

        //position[0] = playerVariables.gameObject.transform.position.x;
        //position[1] = playerVariables.gameObject.transform.position.y;
        //position[2] = playerVariables.gameObject.transform.position.z;
    }
}
