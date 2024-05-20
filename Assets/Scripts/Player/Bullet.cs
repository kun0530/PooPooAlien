using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    private float speed = 10f;
    private float atk = 50f;
    public IObjectPool<Bullet> pool;

    // 테스트
    public DevelopPlayerData playerData;

    private void Start()
    {
        speed = playerData.focusSpeed;
        atk = playerData.focusAttack;
    }
    // 테스트

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            var living = collider.GetComponent<LivingEntity>();
            living.OnDamage(atk);
            if (pool != null)
            {
                pool.Release(this);
            }
        }
        else if (collider.CompareTag("Wall") && pool != null)
        {
            pool.Release(this);
        }
    }

    public void SetAtk(float atk)
    {
        this.atk = atk;
    }
}