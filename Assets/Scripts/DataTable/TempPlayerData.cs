using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/TempPlayerData", fileName = "Temp Player Data")]
public class TempPlayerData : ScriptableObject
{
    public float playerHealth;
    public float bulletAtk;
    public float bulletSpeed;
    public float bulletInterval;
}
