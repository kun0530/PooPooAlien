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
    public Transform firePosition;

    private ShootingMode shootingMode;
    public List<Transform> fireDirections;
    public LineRenderer lazorLineRenderer;
    public GameObject lazorBox;

    public DevelopPlayerData testPlayerData;

    private float nextCreateTime;
    public float interval = 0.2f;

    public float BasicAttack { get; private set; }
    private float itemAttack;
    public float ItemAttack {
        get { return itemAttack; }
        set {
            itemAttack = value;
            FinalAttack = BasicAttack + ItemAttack;
        }
    }
    public float FinalAttack { get; private set; }

    private void Awake()
    {
        var basicAttackLevel = Variables.SaveData.EnhanceStatData[PlayerStat.BasicAttack];
        var basicAttackData = DataTableManager.Get<EnhanceTable>(DataTableIds.Enhance).Get(PlayerStat.BasicAttack);
        BasicAttack = basicAttackData.StatIncrease * (basicAttackLevel - 1); // basic stat 적용해야함...
        FinalAttack = BasicAttack;

        lazorLineRenderer.enabled = false;
        lazorLineRenderer.positionCount = 2;

        lazorBox.SetActive(false);
    }

    private void Start()
    {
        shootingMode = ShootingMode.Focus;

        nextCreateTime = Time.time + interval;

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            shootingMode = ShootingMode.Focus;
            lazorLineRenderer.enabled = false;
            lazorBox.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            shootingMode = ShootingMode.Spread;
            lazorLineRenderer.enabled = false;
            lazorBox.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            shootingMode = ShootingMode.Lazor;
            lazorLineRenderer.enabled = true;
            lazorBox.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            shootingMode = ShootingMode.Penet;
            lazorLineRenderer.enabled = true;
            lazorBox.SetActive(true);
        }

        switch (shootingMode)
        {
            case ShootingMode.Focus:
                CreateFocusBullet();
                break;
            case ShootingMode.Spread:
                CreateSpreadBullet();
                break;
            case ShootingMode.Lazor:
                LazorAttack();
                break;
            case ShootingMode.Penet:
                PenetAttack();
                break;
        }
    }

    private void CreateFocusBullet()
    {
        if (nextCreateTime < Time.time)
        {
            var newBullet = poolBullet.Get();
            newBullet.transform.position = firePosition.position;
            newBullet.transform.LookAt(fireDirections[0].position);
            newBullet.SetAtk(FinalAttack);

            nextCreateTime = Time.time + interval;
        }
    }

    private void CreateSpreadBullet()
    {
        if (nextCreateTime < Time.time)
        {
            foreach (var dir in fireDirections)
            {
                var newBullet = poolBullet.Get();
                newBullet.transform.position = firePosition.position;
                newBullet.transform.LookAt(dir.position);
                newBullet.SetAtk(FinalAttack);
            }

            nextCreateTime = Time.time + interval;
        }
    }

    private void LazorAttack()
    {
        var fireDistance = 100f;
        var hitPoint = Vector3.zero;
        var ray = new Ray(firePosition.position, firePosition.forward);

        int layerMask = 1 << LayerMask.NameToLayer("Monster");
        if (Physics.Raycast(ray, out RaycastHit hitInfo, fireDistance, layerMask))
        {
            hitPoint = hitInfo.point;
        }
        else
        {
            hitPoint = firePosition.position + firePosition.forward * fireDistance;
        }

        lazorLineRenderer.SetPosition(0, firePosition.position);
        lazorLineRenderer.SetPosition(1, hitPoint);
    }

    private void PenetAttack()
    {
        if (lazorBox.activeInHierarchy)
            lazorBox.SetActive(false);

        var fireDistance = 100f;
        var hitPoint = firePosition.position + firePosition.forward * fireDistance;

        lazorLineRenderer.SetPosition(0, firePosition.position);
        lazorLineRenderer.SetPosition(1, hitPoint);

        if (nextCreateTime < Time.time)
        {
            lazorBox.SetActive(true);

            nextCreateTime = Time.time + interval;
        }
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