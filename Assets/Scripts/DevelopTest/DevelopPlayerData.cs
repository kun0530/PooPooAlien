using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/DevelopPlayerData", fileName = "Develop Player Data")]
public class DevelopPlayerData : ScriptableObject
{
    public float maxHp;
    public float startHp;

    public float basicAttack;

    public float focusAttack;
    public float focusSpeed;
    public float focusInterval;

    public float spreadAttack;
    public float spreadSpeed;
    public float spreadInterval;

    public float lazorAttack;
    public float lazorSpeed;
    public float lazorInterval;

    public float penetAttack;
    public float penetInterval;

    public float invincibleDuration;

    public float boosterSpeed;
    public float boosterDuration;
    public float boosterSize;

    public float monsterSpawnInterval;
}
