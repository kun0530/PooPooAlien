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

    private bool isEarned;
    private MeshRenderer meshRenderer;
    private AudioSource itemAudioPlayer;
    public ParticleSystem itemGetEffect;

    private void Awake()
    {
        isEarned = false;
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        itemAudioPlayer = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        meshRenderer.enabled = true;
        isEarned = false;
    }

    private void Start()
    {
        direction = new Vector3(0, 0, -1);
    }

    private void Update()
    {
        if (isEarned)
        {
            if (itemGetEffect.isPlaying)
                return;
            else
                pool?.Release(this);
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (isEarned)
            return;

        if (collider.CompareTag("Player"))
        {
            ApplyItemEffect(collider);
            itemAudioPlayer.Play();
            itemGetEffect.Play();
            meshRenderer.enabled = false;
            isEarned = true;
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
