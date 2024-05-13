using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum MonsterType
{
    None = -1,
    Normal,
    Physic,
    Speed,
    Reaf,
    Projectile,
    Count
};

public class Monster : LivingEntity
{
    public MonsterData monsterData;
    public MonsterType monseterType = MonsterType.None;

    private float speed = 3f;
    private float atk = 50f;
    private Vector3 direction;

    public IObjectPool<Monster> pool;

    public ItemSpawner itemSpawner;

    private void Start()
    {
        direction = -transform.forward;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            collider.GetComponent<LivingEntity>().OnDamage(atk);
        }
        else if (collider.CompareTag("Wall") && pool != null)
        {
            pool.Release(this);
        }
    }

    protected override void OnDie()
    {
        base.OnDie();
        var itemDropData = DataTableManager.Get<ItemDropTable>(DataTableIds.ItemDrop).Get(monsterData.ItemDropId);
        var randomPick = UnityEngine.Random.Range(0f, 1f);
        if (itemDropData.DropChance >= randomPick)
        {
            var itemDropChances = itemDropData.itemDropChances;
            var itemWeights = new List<float>();
            foreach (var itemDropChance in itemDropChances)
            {
                itemWeights.Add(itemDropChance.itemChance);
            }
            var dropItemId = itemDropChances[Utils.WeightedRandomPick(itemWeights)].itemId;
            var dropItemData = DataTableManager.Get<ItemTable>(DataTableIds.Item).Get(dropItemId);
            var dropItemType = (ItemType)dropItemData.ItemType;
            itemSpawner.CreateItem(dropItemType, transform.position);
        }
        if (pool != null)
        {
            pool.Release(this);
        }
    }
}