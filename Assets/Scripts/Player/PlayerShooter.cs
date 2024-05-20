using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Pool;

public enum ShootingMode
{
    Focus,
    Spread,
    Lazor,
    Penet
}

public class PlayerShooter : MonoBehaviour
{
    public Bullet bulletPrefab;
    private IObjectPool<Bullet> poolBullet;

    private Dictionary<ShootingMode, Weapon> weapons = new Dictionary<ShootingMode, Weapon>();
    private ShootingMode shootingMode;
    public ShootingMode Mode{
        get { return shootingMode; }
        set {
            shootingMode = value;
            foreach (var weapon in weapons)
            {
                if (weapon.Value == null)
                    continue;

                if (weapon.Key == shootingMode)
                    weapon.Value.gameObject.SetActive(true);
                else
                    weapon.Value.gameObject.SetActive(false);
            }
        }
    }
    private int weaponLevel;
    public int WeaponLevel{
        get { return weaponLevel; }
        set {
            weaponLevel = Mathf.Clamp(value, 1, 3);
        }
    }

    public DevelopPlayerData testPlayerData;

    public float BasicAttack { get; private set; }
    private float powerUpAttack;
    public float PowerUpAttack {
        get { return powerUpAttack; }
        set {
            powerUpAttack = value;
            FinalAttack = BasicAttack + PowerUpAttack;
        }
    }
    public float FinalAttack { get; private set; }

    private void Awake()
    {
        var basicAttackLevel = Variables.SaveData.EnhanceStatData[PlayerStat.BasicAttack];
        var basicAttackData = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance).Get(PlayerStat.BasicAttack);
        BasicAttack = basicAttackData.BasicStat + basicAttackData.StatIncrease * (basicAttackLevel - 1);
        FinalAttack = BasicAttack;
    }

    private void Start()
    {
        weapons.Add(ShootingMode.Focus, GetComponentInChildren<WeaponFocus>());
        weapons.Add(ShootingMode.Spread, GetComponentInChildren<WeaponSpread>());
        weapons.Add(ShootingMode.Lazor, GetComponentInChildren<WeaponLazor>());
        weapons.Add(ShootingMode.Penet, GetComponentInChildren<WeaponPenet>());
        Mode = ShootingMode.Focus;

        WeaponLevel = 3;

        poolBullet = new ObjectPool<Bullet>(
            CreatePooledItem,
            OnTakeFromPool,
            OnReturnToPool,
            OnDestroyPoolObject,
            true, 10, 100
        );

        for (int i = 0; i < 10; i++)
        {
            CreatePooledItem().gameObject.SetActive(false);
        }

        ApplyTestData();
    }

    private void Update()
    {

    }

    public Bullet CreateBullet()
    {
        return poolBullet.Get();
    }

    private Bullet CreatePooledItem()
    {
        var bullet = Instantiate(bulletPrefab);
        bullet.pool = poolBullet;
        return bullet;
    }

    private void OnTakeFromPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(true);
    }

    private void OnReturnToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(Bullet bullet)
    {
        Destroy(bullet);
    }

    [Conditional("DEVELOP_TEST")]
    public void ApplyTestData()
    {
        Logger.Log("Player Shooter: 테스트용 데이터 적용 중");
        BasicAttack = testPlayerData.basicAttack;
    }
}