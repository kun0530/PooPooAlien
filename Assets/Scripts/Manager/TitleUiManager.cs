using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UiStatus
{
    None = -1,
    Title,
    StageSelect,
    Enhance,
    Count
}

public class TitleUiManager : MonoBehaviour
{
    public GameObject titlePanel;
    public GameObject stageSelectPanel;
    public GameObject enhanceStatPanel;

    private Dictionary<UiStatus, GameObject> uiPanels = new Dictionary<UiStatus, GameObject>();

    private UiStatus status;
    public UiStatus Status {
        get { return status; }
        set {
            foreach (var panel in uiPanels)
            {
                if (panel.Value != null)
                panel.Value.SetActive(false);
            }

            if (uiPanels[value] != null)
            {
                status = value;
                uiPanels[status].SetActive(true);
            }
        }
    }

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    private void Start()
    {
        uiPanels.Add(UiStatus.Title, titlePanel);
        uiPanels.Add(UiStatus.StageSelect, stageSelectPanel);
        uiPanels.Add(UiStatus.Enhance, enhanceStatPanel);

        if (Variables.isStartGame)
        {
            Status = UiStatus.Title;
            Variables.isStartGame = false;
        }
        else
        {
            Status = UiStatus.StageSelect;
        }
    }

    private void Update()
    {
    }

    public void SaveFile()
    {
        SaveLoadSystem.Save(Variables.SaveData);
    }
}
