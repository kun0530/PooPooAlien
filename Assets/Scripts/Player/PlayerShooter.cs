using System.Collections;
using System.Collections.Generic;
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

    private float nextCreateTime;
    private float interval = 0.1f;

    public TempPlayerData playerData; // 테스트
    public float atk { get; set; }

    private void Awake()
    {
        lazorLineRenderer.enabled = false;
        lazorLineRenderer.positionCount = 2;
    }

    private void Start()
    {
        interval = playerData.bulletInterval; // 테스트
        atk = playerData.bulletAtk; // 테스트

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            shootingMode = ShootingMode.Focus;
            lazorLineRenderer.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            shootingMode = ShootingMode.Spread;
            lazorLineRenderer.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            shootingMode = ShootingMode.Lazor;
            lazorLineRenderer.enabled = true;
        }

        if (shootingMode == ShootingMode.Lazor)
        {
            var fireDistance = 100f;
            var hitPoint = Vector3.zero;
            var ray = new Ray(firePosition.position, firePosition.forward);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, fireDistance))
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

        if (nextCreateTime < Time.time)
        {
            switch (shootingMode)
            {
                case ShootingMode.Focus:
                    CreateFocusBullet();
                    break;
                case ShootingMode.Spread:
                    CreateSpreadBullet();
                    break;
                case ShootingMode.Lazor:
                    CreateLazorBullet();
                    break;
                case ShootingMode.Penet:
                    break;
            }

            

            nextCreateTime = Time.time + interval;
        }
    }

    private void CreateFocusBullet()
    {
        var newBullet = poolBullet.Get();
        newBullet.transform.position = firePosition.position;
        newBullet.transform.LookAt(fireDirections[0].position);
        newBullet.SetAtk(atk);
    }

    private void CreateSpreadBullet()
    {
        foreach (var dir in fireDirections)
        {
            var newBullet = poolBullet.Get();
            newBullet.transform.position = firePosition.position;
            newBullet.transform.LookAt(dir.position);
            newBullet.SetAtk(atk);
        }
    }

    private void CreateLazorBullet()
    {
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
}