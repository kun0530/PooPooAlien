using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

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
            isBounce = data.IsBounce;
            direction = new Vector3(hSpeed, 0, -vSpeed);
        }
    }

    private float atk;
    private float def;

    private float vSpeed;
    private float hSpeed;
    private Vector3 direction;

    public IObjectPool<Monster> pool;

    public GameManager gameManager;

    public bool isDamageAble { get; set; }

    private float moveLimitLeft;
    private float moveLimitRight;

    private bool isBounce;

    protected override void OnEnable()
    {
        base.OnEnable();
        isDamageAble = false;
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        var spawnPositions = gameManager.monsterSpawner.spawnPositions;
        moveLimitLeft = spawnPositions[0].position.x;
        moveLimitRight = spawnPositions[spawnPositions.Count() - 1].position.x;
        // direction = new Vector3(1, 0, -1);
    }

    private void Update()
    {
        transform.position += direction * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        if (isBounce && direction.x != 0)
        {
            if ((transform.position.x <= moveLimitLeft && direction.x < 0)
            || (transform.position.x >= moveLimitRight && direction.x > 0)) // test 스포너 양끝 경계
            {
                direction.x *= -1;
            }
        }
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
        if (!isDamageAble)
            return;

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