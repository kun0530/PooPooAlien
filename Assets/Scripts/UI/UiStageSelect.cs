using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiStageSelect : MonoBehaviour
{
    public Camera mainCamera;
    public Transform cameraTarget;
    private Vector3 nextPlanetPos;
    private float cameraSpeed = 10f;
    private bool isCameraMoving;

    public List<Transform> planets;
    private int currentStage;

    private void Start()
    {
        currentStage = 0;
        cameraTarget.position = planets[currentStage].position;
        mainCamera.transform.LookAt(cameraTarget);
        isCameraMoving = false;
    }

    private void Update()
    {
        if (isCameraMoving)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, nextPlanetPos, Time.deltaTime * cameraSpeed);
            if (Vector3.Distance(cameraTarget.position, nextPlanetPos) < 1f)
            {
                cameraTarget.position = nextPlanetPos;
                isCameraMoving = false;
            }
            mainCamera.transform.LookAt(cameraTarget);
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            currentStage++;
            if (currentStage >= planets.Count)
                currentStage = 0;
            nextPlanetPos = planets[currentStage].position;

            isCameraMoving = true;
        }
    }
}
