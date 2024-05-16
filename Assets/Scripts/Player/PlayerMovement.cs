using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 move;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private bool isMoving = false;
    // private float speed = 5f;

    private float moveLimitLeft;
    private float moveLimitRight;

    private void Awake()
    {
        isMoving = false;
    }

    private void Start()
    {
        var gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        var spawnPositions = gameManager.monsterSpawner.spawnPositions;
        moveLimitLeft = spawnPositions[0].position.x;
        moveLimitRight = spawnPositions[spawnPositions.Count() - 1].position.x;
    }

    private void Update()
    {
        move = Vector3.zero;

        if (!isMoving && Input.GetMouseButtonDown(0))
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cameraRay, out RaycastHit hitInfo))
            {
                if (hitInfo.collider.gameObject == gameObject)
                {
                    isMoving = true;
                }
            }
        }

        if (isMoving && Input.GetMouseButton(0))
        {
            var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(cameraRay, out float rayLength))
            {
                move = new Vector3(Mathf.Clamp(cameraRay.GetPoint(rayLength).x, moveLimitLeft, moveLimitRight), 0f, transform.position.z);
                transform.position = move;
                // transform.position = Vector3.Lerp(transform.position, move, Time.deltaTime * speed);
            }
        }

        if (isMoving && Input.GetMouseButtonUp(0))
        {
            isMoving = false;
            // 이동 끝처리는 조금 더 생각
        }
    }
}
