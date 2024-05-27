using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UiStates
{
    None = -1,
    Title,
    StageSelect,
    Setting,
    Enhance,
    Count
}

public class TitleUiManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject stageSelectPanel;
    public GameObject enhanceStatPanel;
    public GameObject settingPanel;

    private Dictionary<UiStates, GameObject> uiPanels = new Dictionary<UiStates, GameObject>();

    public UiStates UiState { get; private set; } = UiStates.None;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        uiPanels.Add(UiStates.Title, titlePanel);
        uiPanels.Add(UiStates.StageSelect, stageSelectPanel);
        uiPanels.Add(UiStates.Setting, settingPanel);
        uiPanels.Add(UiStates.Enhance, enhanceStatPanel);

        if (Variables.isStartGame)
        {
            ChangeUiState(UiStates.Title);
            Variables.isStartGame = false;
        }
        else
        {
            ChangeUiState(UiStates.StageSelect);
        }
    }

    private void Update()
    {
    }

    public void SaveFile()
    {
        SaveLoadSystem.Save(Variables.SaveData);
    }

    [VisibleEnum(typeof(UiStates))]
    public void ChangeUiState(int state)
    {
        if (UiState == (UiStates)state || state <= (int)UiStates.None || state >= (int)UiStates.Count)
            return;

        foreach (var panel in uiPanels)
        {
            panel.Value?.SetActive(false);
        }

        uiPanels[(UiStates)state]?.SetActive(true);
        UiState = (UiStates)state;
    }

    public void ChangeUiState(UiStates state)
    {
        ChangeUiState((int)state);
    }
}
