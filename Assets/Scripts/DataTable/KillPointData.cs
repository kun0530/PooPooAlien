using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable/FillPointData", fileName = "Kill Point Data")]
public class KillPointData : ScriptableObject
{
    public List<float> killPointBoundaries;
}