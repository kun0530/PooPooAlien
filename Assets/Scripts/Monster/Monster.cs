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
    private MonsterData data;
    public MonsterData Data{
        get { return data; }
        set {
            data = value;
            startHealth = data.Hp;
            currentHealth = startHealth;
            atk = data.Atk;
            def = data.Def;
            vSpeed = data.VerticalSpd;
            hSpeed = data.HorizontalSpd;
            direction = new Vector3(hSpeed, 0, -vSpeed);
        }
    }
    public MonsterType monseterType = MonsterType.None;

    private float atk;
    private float def;

    private float vSpeed;
    private float hSpeed;
    private Vector3 direction;

    public IObjectPool<Monster> pool;

    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // direction = new Vector3(1, 0, -1);
    }

    private void Update()
    {
        transform.position += direction * Time.deltaTime;
        transform.LookAt(transform.position + direction);
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

    public override void OnDamage(float damage)
    {
        damage -= def;
        if (damage > 0)
            base.OnDamage(damage);
    }

    protected override void OnDie()
    {
        base.OnDie();

        gameManager.AddScore(Data.Score);
        gameManager.AddKillPoint(Data.KillPoint);

        DropItem();

        if (pool != null)
        {
            pool.Release(this);
        }
    }

    private void DropItem()
    {
        var itemDropData = DataTableManager.Get<ItemDropTable>(DataTableIds.ItemDrop).Get(Data.ItemDropId);
        var randomPick = UnityEngine.Random.Range(0f, 1f);
        if (itemDropData.DropChance < randomPick)
            return;

        var itemDropChances = itemDropData.itemDropChances;
        var itemWeights = new List<float>();
        foreach (var itemDropChance in itemDropChances)
        {
            itemWeights.Add(itemDropChance.itemChance);
        }
        var dropItemId = itemDropChances[Utils.WeightedRandomPick(itemWeights)].itemId;
        var dropItemData = DataTableManager.Get<ItemTable>(DataTableIds.Item).Get(dropItemId);
        var dropItemType = (ItemType)dropItemData.ItemType;
        gameManager.itemSpawner.CreateItem(dropItemType, transform.position);
    }
}