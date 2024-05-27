using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public enum ItemType
{
    None = -1,
    Heal,
    PowerUp,
    WeaponFocus,
    WeaponSpread,
    WeaponLazor,
    WeaponPenet,
    Booster,
    Count
}

public class Item : MonoBehaviour
{
    public ItemType itemType { get; set; }
    public ItemData data { get; set; }

    public IObjectPool<Item> pool;

    private float speed = 3f;
    private Vector3 direction;

    private AudioSource itemAudioPlayer;

    private void Awake()
    {
        itemAudioPlayer = GetComponent<AudioSource>();
    }

    private void Start()
    {
        direction = new Vector3(0, 0, -1);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            ApplyItemEffect(collider);
            itemAudioPlayer.Play();
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

    protected virtual void ApplyItemEffect(Collider player)
    {
    }
}
