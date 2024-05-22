using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class UiStageSelect : MonoBehaviour
{
    public TitleUiManager uiManager;

    public Camera mainCamera;
    public Transform cameraTarget;
    private Vector3 nextPlanetPos;
    private float cameraSpeed = 10f;
    private bool isCameraMoving;

    public List<Transform> planets;
    private int startStage = 1;
    private int stageCount;

    private int selectedStage;
    public int SelectedStage{
        get{ return selectedStage; }
        set{
            if (isCameraMoving)
                return;

            selectedStage = Mathf.Clamp(value, startStage, stageCount);
            nextPlanetPos = planets[selectedStage].position;
            isCameraMoving = true;
        }
    }

    private void Start()
    {
        var stageTable = DataTableManager.Get<StageTable>(DataTableIds.Stage);
        stageCount = planets.Count - startStage > stageTable.CountStage() ? stageTable.CountStage() : planets.Count - startStage;
        Logger.Log($"스테이지 개수: {stageCount}");

        selectedStage = startStage;
        cameraTarget.position = planets[selectedStage].position;
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
    }

    public void SelectNextStage(bool isNext)
    {
        SelectedStage += isNext ? 1 : -1;
        Logger.Log($"Selected Stage: {SelectedStage}");
    }

    public void EnterStage()
    {
        Variables.stageId = selectedStage;
        SceneManager.LoadScene(SceneIds.Develop);
    }

    public void EnterEnhance()
    {
        uiManager.Status = UiStatus.Enhance;
    }
}
