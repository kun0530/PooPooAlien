using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
            var playerShooter = collider.GetComponent<PlayerShooter>();
            playerShooter.WeaponLevel--;
            playerShooter.PowerUpCount = 0;
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

        gameManager.CurrentScore += Data.Score;
        gameManager.CurrentKillPoint += Data.KillPoint;
        gameManager.EarnedGold += Data.GetGold;
        gameManager.itemSpawner.DropItem(Data.ItemDropId, transform.position);

        if (pool != null)
        {
            pool.Release(this);
        }
    }
}