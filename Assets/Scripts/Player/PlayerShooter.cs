using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Pool;

public enum WeaponType
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

    private Dictionary<WeaponType, Weapon> weapons = new Dictionary<WeaponType, Weapon>();
    private WeaponType currentWeaponType;
    public WeaponType CurrentWeaponType{
        get { return currentWeaponType; }
        set {
            currentWeaponType = value;
            foreach (var weapon in weapons)
            {
                if (weapon.Value == null)
                    continue;

                if (weapon.Key == currentWeaponType)
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
            foreach (var weapon in weapons)
            {
                weapon.Value.ChangeWeaponPhase(weapon.Key);
            }
        }
    }

    public DevelopPlayerData testPlayerData;

    public float BasicAttack { get; private set; }
    public float PowerUpAttack { get; private set; }
    private int powerUpCount;
    public int PowerUpCount {
        get { return powerUpCount; }
        set {
            powerUpCount = Mathf.Clamp(value, 0, 3);
            FinalAttack = BasicAttack + powerUpCount * PowerUpAttack;
        }
    }
    public float FinalAttack { get; private set; }

    private void Awake()
    {
        BasicAttack = Variables.CalculateSaveStat(PlayerStat.BasicAttack);
        PowerUpAttack = Variables.CalculateSaveStat(PlayerStat.PowerUpDamage);
        powerUpCount = 0;
        FinalAttack = BasicAttack;
    }

    private void Start()
    {
        weapons.Add(WeaponType.Focus, GetComponentInChildren<WeaponFocus>());
        weapons.Add(WeaponType.Spread, GetComponentInChildren<WeaponSpread>());
        weapons.Add(WeaponType.Lazor, GetComponentInChildren<WeaponLazor>());
        weapons.Add(WeaponType.Penet, GetComponentInChildren<WeaponPenet>());
        CurrentWeaponType = WeaponType.Focus;

        WeaponLevel = 1;

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
        WeaponChangeTest();
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

    public void ChangeOrUpgradeWeapon(WeaponType type)
    {
        if (CurrentWeaponType == type)
        {
            WeaponLevel++;
            return;
        }
        
        CurrentWeaponType = type;
    }

    [Conditional("DEVELOP_TEST")]
    public void WeaponChangeTest()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeOrUpgradeWeapon(WeaponType.Focus);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeOrUpgradeWeapon(WeaponType.Spread);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeOrUpgradeWeapon(WeaponType.Lazor);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeOrUpgradeWeapon(WeaponType.Penet);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            gameObject.GetComponent<PlayerBooster>().enabled = true;
        }
    }

    [Conditional("DEVELOP_TEST")]
    public void ApplyTestData()
    {
        if (!testPlayerData.isTesting)
            return;

        Logger.Log("Player Shooter: 테스트용 데이터 적용 중");
        BasicAttack = testPlayerData.basicAttack;
    }
}