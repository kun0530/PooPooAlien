using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum ItemType
{
    None = -1,
    Heal,
    PowerUp,
    Coin,
    Weapon,
    Booster,
    Count
}

public class Item : MonoBehaviour
{
    public ItemType itemType = ItemType.None;

    public IObjectPool<Item> pool;

    private float speed = 3f;
    private Vector3 direction;

    private void Start()
    {
        direction = -transform.forward;
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (pool != null)
            {
                pool.Release(this);
            }
        }
        else if (collider.CompareTag("Wall") && pool != null)
        {
            pool.Release(this);
        }
    }
}
