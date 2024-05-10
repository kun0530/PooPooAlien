using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public ItemSpawner itemSpawner;

    public void SpawnItem(Vector3 item)
    {
        itemSpawner.CreateItem(item);
    }
}
