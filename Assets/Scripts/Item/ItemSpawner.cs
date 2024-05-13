using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ItemSpawner : MonoBehaviour
{
    public Item[] itemPrefabs;
    private Dictionary<ItemType, IObjectPool<Item>> poolItems = new Dictionary<ItemType,IObjectPool<Item>>();
    // private IObjectPool<Item> poolItem;

    private void Start()
    {
        for (int i = 0; i < (int)ItemType.Count; i++)
        {
            int index = i;
            var itemType = (ItemType)index;

            IObjectPool<Item> poolItem = new ObjectPool<Item>(
                () => {
                    var item = Instantiate(itemPrefabs[index]);
                    item.itemType = itemType;
                    item.pool = poolItems[itemType];
                    return item;
                },
                OnTakeFromPool,
                OnReturnToPool,
                OnDestroyPoolObject,
                true, 10, 100
            );

            poolItems.Add(itemType, poolItem);
        }
    }

    public void CreateItem(ItemType type, Vector3 pos)
    {
        var newItem = poolItems[type].Get();
        newItem.transform.position = pos;
    }

    // private Item CreatePooledItem()
    // {
    //     var item = Instantiate(itemPrefab);
    //     item.pool = poolItem;
    //     return item;
    // }

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