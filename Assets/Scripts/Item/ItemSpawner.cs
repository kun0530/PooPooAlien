using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemSpawner : MonoBehaviour
{
    public Item itemPrefab;
    private IObjectPool<Item> poolItem;

    private void Start()
    {
        poolItem = new ObjectPool<Item>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            true, 10, 100
        );
    }

    public void CreateItem(Vector3 pos)
    {
        var newItem = poolItem.Get();
        newItem.transform.position = pos;
    }

    private Item CreatePooledItem()
    {
        var item = Instantiate(itemPrefab);
        item.pool = poolItem;
        return item;
    }

    private void OnTakeFromPool(Item item)
    {
        item.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Item item)
    {
        item.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Item item)
    {
        Destroy(item);
    }
}