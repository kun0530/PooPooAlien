using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Enemy : MonoBehaviour
{
    private float speed = 3f;
    private Vector3 direction;

    public IObjectPool<Enemy> pool;

    private void Start()
    {
        direction = -transform.forward;
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Wall") && pool != null)
        {
            pool.Release(this);
        }
    }
}