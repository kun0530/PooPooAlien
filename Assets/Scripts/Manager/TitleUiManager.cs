using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUiManager : MonoBehaviour
{
    public Camera mainCamera;
    public Transform cameraTarget;
    private Vector3 nextPlanetPos;
    private float cameraSpeed = 10f;
    private bool isCameraMoving;

    public List<Button> buttons;
    public List<Transform> planets;
    private int currentStage;

    private void Start()
    {
        foreach (var button in buttons)
        {
            var buttonImage = button.GetComponent<Image>();
            if (buttonImage != null)
                buttonImage.alphaHitTestMinimumThreshold = 0.1f;
        }

        currentStage = 0;
        cameraTarget.position = planets[currentStage].position;
        mainCamera.transform.LookAt(cameraTarget);
        isCameraMoving = false;
    }

    // Update is called once per frame
    void Update()
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

    public void TestClick()
    {
        Logger.Log("Click");
    }
}
