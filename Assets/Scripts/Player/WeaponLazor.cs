using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLazor : Weapon
{
    public Transform firePosition;
    private RaycastHit hitInfo;
    private bool isHitted;

    private LineRenderer lazorLineRenderer;

    public Material[] laserPhase01Materials;
    public Material[] laserPhase02Materials;
    public Material[] laserPhase03Materials;
    private List<Material[]> laserMaterials = new List<Material[]>();

    private Vector3 hitPointOffset = new Vector3(0f, 0f, -0.3f);

    protected override void Awake()
    {
        lazorLineRenderer = GetComponent<LineRenderer>();
        lazorLineRenderer.positionCount = 2;
        isHitted = false;

        base.Awake();

        weaponAttack = Variables.CalculateCurrentSaveStat(PlayerStat.LazorAttack);
    }

    protected override void Start()
    {
        base.Start();

        laserMaterials.Add(laserPhase01Materials);
        laserMaterials.Add(laserPhase02Materials);
        laserMaterials.Add(laserPhase03Materials);
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
            hitPoint = hitInfo.point + hitPointOffset;
        }
        else
        {
            isHitted = false;
            hitPoint = firePosition.position + firePosition.forward * fireDistance;
        }
        lazorLineRenderer.materials = laserMaterials[playerShooter.WeaponLevel - 1];
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
            monster.OnDamage(playerShooter.FinalAttack + weaponAttack + weaponPhaseAttack);
    }

    public override void ApplyTestData()
    {
        if (!playerShooter.testPlayerData.isTesting)
            return;

        weaponAttack = playerShooter.testPlayerData.lazorAttack;
        weaponInterval = playerShooter.testPlayerData.lazorInterval;

        weaponPhaseAttack = 0f;
    }
}
