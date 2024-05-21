using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLazor : Weapon
{
    public Transform firePosition;
    private RaycastHit hitInfo;
    private bool isHitted;

    private LineRenderer lazorLineRenderer;

    protected override void Awake()
    {
        lazorLineRenderer = GetComponent<LineRenderer>();
        lazorLineRenderer.positionCount = 2;
        isHitted = false;

        base.Awake();

        weaponAttack = Variables.CalculateSaveStat(PlayerStat.LazorAttack);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        var fireDistance = 100f;
        var hitPoint = Vector3.zero;
        var ray = new Ray(firePosition.position, firePosition.forward);

        int layerMask = 1 << LayerMask.NameToLayer("Monster");
        if (Physics.Raycast(ray, out hitInfo, fireDistance, layerMask))
        {
            isHitted = true;
            hitPoint = hitInfo.point;
        }
        else
        {
            isHitted = false;
            hitPoint = firePosition.position + firePosition.forward * fireDistance;
        }

        lazorLineRenderer.SetPosition(0, firePosition.position);
        lazorLineRenderer.SetPosition(1, hitPoint);

        base.Update();
    }

    protected override void Fire()
    {
        if (!isHitted)
            return;

        var monster = hitInfo.collider.GetComponent<Monster>();
        if (monster != null)
            monster.OnDamage(playerShooter.FinalAttack + weaponAttack);
    }

    public override void ApplyTestData()
    {
        if (!playerShooter.testPlayerData.isTesting)
            return;

        weaponAttack = playerShooter.testPlayerData.lazorAttack;
        weaponInterval = playerShooter.testPlayerData.lazorInterval;
    }
}
