using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPenet : Weapon
{
    public GameObject penetCube;
    public Transform firePosition;
    public Transform fireDirection;

    public LineRenderer penetLineRenderer;

    protected override void Awake()
    {
        penetCube.SetActive(false);

        penetLineRenderer = GetComponent<LineRenderer>();
        penetLineRenderer.positionCount = 2;

        base.Awake();
    }

    protected override void Update()
    {
        if (penetCube.activeInHierarchy)
            penetCube.SetActive(false);

        var fireDistance = 100f;
        var hitPoint = firePosition.position + fireDirection.position * fireDistance;

        penetLineRenderer.SetPosition(0, firePosition.position);
        penetLineRenderer.SetPosition(1, hitPoint);

        base.Update();
    }

    protected override void Fire()
    {
        penetCube.SetActive(true);
    }
}
