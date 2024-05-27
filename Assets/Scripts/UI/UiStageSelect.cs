using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiStageSelect : MonoBehaviour
{
    public TitleUiManager uiManager;
    private StageTable stageTable;

    public Camera mainCamera;
    public Transform cameraTarget;
    private Vector3 nextPlanetPos;
    public float cameraSpeed = 8f;
    private Vector3 nextCameraPos;
    private bool isTargetMoving;
    private bool isCameraMoving;

    public TextMeshProUGUI stageNameText;
    private string stageNameForamt = "{0}. {1}";
    public List<Image> stageStars;

    public List<Transform> planets;
    public List<Transform> cameraPositions;
    private int startStage = 1;
    private int stageCount;

    public Button stageStartButton;

    private int selectedStage;
    public int SelectedStage{
        get{ return selectedStage; }
        set{
            if (isTargetMoving || isCameraMoving)
                return;

            if (value > stageCount)
                value = startStage;
            else if (value < startStage)
                value = stageCount;

            selectedStage = value;

            stageNameText.text = string.Format(stageNameForamt, selectedStage, stageTable.Get(selectedStage).GetName());
            
            var clearStar = Variables.SaveData.StageClearData[selectedStage];
            foreach (var star in stageStars)
            {
                star.enabled = false;
            }
            if (clearStar != -1)
                stageStars[Variables.SaveData.StageClearData[selectedStage]].enabled = true;

            if (selectedStage >= startStage)
            {
                var clearPrevStar = Variables.SaveData.StageClearData[selectedStage - 1];
                stageStartButton.interactable = clearPrevStar >= 0 || selectedStage == startStage;
            }

            nextPlanetPos = planets[selectedStage].position;
            isTargetMoving = true;
            nextCameraPos = cameraPositions[selectedStage].position;
            isCameraMoving = true;
        }
    }

    private void Awake()
    {
        stageTable = DataTableManager.Get<StageTable>(DataTableIds.Stage);
    }

    private void Start()
    {
        stageCount = planets.Count - startStage > stageTable.CountStage() ? stageTable.CountStage() : planets.Count - startStage;
        Logger.Log($"스테이지 개수: {stageCount}");

        SelectedStage = Variables.stageId > startStage && Variables.stageId <= stageCount ? Variables.stageId : startStage;
        cameraTarget.position = planets[SelectedStage].position;
        mainCamera.transform.position = cameraPositions[SelectedStage].position;
        mainCamera.transform.LookAt(cameraTarget);
        isTargetMoving = false;
        isCameraMoving = false;
    }

    private void Update()
    {
        if (TouchManager.Instance.Swipe == Directions.Right)
        {
            SelectedStage--;
        }
        else if (TouchManager.Instance.Swipe == Directions.Left)
        {
            SelectedStage++;
        }

        if (isCameraMoving)
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, nextCameraPos, Time.deltaTime * cameraSpeed);
            if (Vector3.Distance(mainCamera.transform.position, nextCameraPos) < 0.01f)
            {
                mainCamera.transform.position = nextCameraPos;
                isCameraMoving = false;
            }
        }

        if (isTargetMoving)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, nextPlanetPos, Time.deltaTime * cameraSpeed);
            if (Vector3.Distance(cameraTarget.position, nextPlanetPos) < 0.01f)
            {
                cameraTarget.position = nextPlanetPos;
                isTargetMoving = false;
            }
        }

        if (isCameraMoving || isTargetMoving)
        {
            mainCamera.transform.LookAt(cameraTarget);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.ChangeUiState(UiStates.Title);
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
        uiManager.ChangeUiState(UiStates.Enhance);
    }
}
