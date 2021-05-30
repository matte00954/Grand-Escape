//Main author: Mattias Larsson

[System.Serializable]
public class PlayerData
{

    public string level;

    public int savedHealthPoints;
    public int savedAmmoCount;
    public int savedSelectedWeapon;

    public float savedStaminaPoints;
    public float[] position; //Can not save vector3 here, instead will save 3 floats, x y z

    public PlayerData(PlayerVariables playerVariables, string currentLevel)
    {
        level = currentLevel;
        savedHealthPoints = playerVariables.GetCurrentHealthPoints();
        savedStaminaPoints = playerVariables.GetCurrentStamina();
        savedAmmoCount = playerVariables.GetCurrentAmmoReserve();

        position = new float[3];
        position[0] = playerVariables.gameObject.transform.position.x;
        position[1] = playerVariables.gameObject.transform.position.y;
        position[2] = playerVariables.gameObject.transform.position.z;
    }
}
