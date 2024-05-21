using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponPenet : Weapon
{
    public GameObject penetCube;
    private PenetCube getPenetCube;
    public Transform firePosition;

    private LineRenderer penetLineRenderer;

    protected override void Awake()
    {
        penetCube.SetActive(false);

        penetLineRenderer = GetComponent<LineRenderer>();
        penetLineRenderer.positionCount = 2;

        base.Awake();

        weaponAttack = Variables.CalculateSaveStat(PlayerStat.PenetAttack);
    }

    protected override void Start()
    {
        base.Start();

        getPenetCube = penetCube.GetComponent<PenetCube>();
    }

    protected override void Update()
    {
        if (penetCube.activeInHierarchy)
            penetCube.SetActive(false);

        var fireDistance = 100f;
        var hitPoint = firePosition.position + firePosition.forward * fireDistance;

        penetLineRenderer.SetPosition(0, firePosition.position);
        penetLineRenderer.SetPosition(1, hitPoint);

        base.Update();
    }

    protected override void Fire()
    {
        penetCube.transform.localScale = new Vector3(weaponScale, penetCube.transform.localScale.y, penetCube.transform.localScale.z);
        getPenetCube.penetAttack = playerShooter.FinalAttack + weaponAttack + weaponPhaseAttack;
        penetCube.SetActive(true);
    }

    public override void ApplyTestData()
    {
        if (!playerShooter.testPlayerData.isTesting)
            return;

        weaponAttack = playerShooter.testPlayerData.penetAttack;
        weaponScale = playerShooter.testPlayerData.penetScale;
        weaponInterval = playerShooter.testPlayerData.penetInterval;
    }
}
