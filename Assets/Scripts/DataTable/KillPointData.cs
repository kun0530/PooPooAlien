using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/FillPointData", fileName = "Kill Point Data")]
public class KillPointData : ScriptableObject
{
    public List<float> killPointBoundaries;
}

[CreateAssetMenu(menuName = "Scriptable/TempPlayerData", fileName = "Temp Player Data")]
public class TempPlayerData : ScriptableObject
{
    public float playerHealth;
    public float bulletAtk;
    public float bulletSpeed;
    public float bulletInterval;
}