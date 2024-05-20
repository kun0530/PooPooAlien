using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFocus : Weapon
{
    public Transform[] firePositions;
    public Transform[] fireDirections;

    private bool isFiring;
    private int bulletIndex;
    private float bulletInterval = 0.1f;
    private float nextBulletTime;

    protected override void Awake()
    {
        base.Awake();

        isFiring = false;
        bulletIndex = 0;
        nextBulletTime = 0f;
    }

    protected override void Update()
    {
        base.Update();

        if (isFiring && nextBulletTime <= Time.time)
        {
            if (bulletIndex < playerShooter.WeaponLevel)
            {
                FireBullets();
                bulletIndex++;
                nextBulletTime = Time.time + bulletInterval;
            }
            else
            {
                isFiring = false;
                bulletIndex = 0;
                nextBulletTime = 0f;
            }
        }
    }

    protected override void Fire()
    {
        isFiring = true;
    }

    private void FireBullets()
    {
        for (int i = 0; i < 2; i++)
        {
            var newBullet = playerShooter.CreateBullet();
            newBullet.transform.position = firePositions[i].position;
            newBullet.transform.LookAt(fireDirections[i].position);
            newBullet.SetAtk(playerShooter.FinalAttack + weaponAttack);
        }
    }
}
