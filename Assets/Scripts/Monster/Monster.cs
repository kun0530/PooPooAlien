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
            if (value == null)
                return;

            data = value;
            startHealth = data.Hp;
            currentHealth = startHealth;
            atk = data.Atk;
            def = data.Def;
            vSpeed = data.VerticalSpd;
            hSpeed = data.HorizontalSpd;
            isBounce = data.IsBounce;
            var material = data.GetMaterial();
            if (material != null)
                skinRenderer.material = material;
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

    private SkinnedMeshRenderer skinRenderer;

    public ParticleSystem hitEffect;
    public ParticleSystem deathEffect;

    private AudioSource audioPlayer;
    public AudioClip hitSound;
    public AudioClip deathSound;

    private void Awake()
    {
        skinRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        audioPlayer = GetComponent<AudioSource>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        skinRenderer.enabled = true;
        isDamageAble = false;

        gameObject.layer = LayerMask.NameToLayer("Monster");
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
        if (isDead)
        {
            if (deathEffect.isPlaying)
                return;
            else
                pool?.Release(this);
        }

        transform.position += direction * Time.deltaTime;
        transform.LookAt(transform.position + direction);

        if (isBounce && direction.x != 0)
        {
            if ((transform.position.x <= moveLimitLeft && direction.x < 0)
            || (transform.position.x >= moveLimitRight && direction.x > 0))
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
        if (!isDamageAble || isDead)
            return;

        hitEffect?.Play();
        if (hitSound != null)
            audioPlayer?.PlayOneShot(hitSound);

        damage -= def;
        if (damage > 0)
            base.OnDamage(damage);
    }

    public override void OnDie()
    {
        if (isDead)
            return;

        base.OnDie();

        gameObject.layer = LayerMask.NameToLayer("DeadMonster");

        skinRenderer.enabled = false;
        hitEffect?.Stop();
        deathEffect?.Play();

        audioPlayer?.Stop();
        if (deathSound != null)
            audioPlayer?.PlayOneShot(deathSound);

        gameManager.CurrentScore += Data.Score;
        gameManager.CurrentKillPoint += Data.KillPoint;
        gameManager.EarnedGold += Data.GetGold;
        gameManager.itemSpawner.DropItem(Data.ItemDropId, transform.position);
    }
}