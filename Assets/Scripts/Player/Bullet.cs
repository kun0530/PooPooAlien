using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour
{
    public float speed { get; set; }
    public float atk { get; set; }
    public IObjectPool<Bullet> pool;

    private void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            var living = collider.GetComponent<LivingEntity>();
            if (living != null)
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
}