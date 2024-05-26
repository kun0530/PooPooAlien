using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;

public class StageUiManager : MonoBehaviour
{
    private GameManager gameManager;

    public GameObject stagePanel;
    public TextMeshProUGUI textGameTimer;
    public List<Image> heartImages;

    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public GameObject gameClearPanel;
    public Dictionary<GameState, GameObject> panels = new Dictionary<GameState, GameObject>();
    private GameObject currentActivePanel;
    public GameObject CurrentActivePanel
    {
        get { return currentActivePanel; }
        set {
            currentActivePanel?.SetActive(false);
            currentActivePanel = value;
            currentActivePanel?.SetActive(true);
        }
    }

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        panels.Add(GameState.Pause, pausePanel);
        panels.Add(GameState.GameOver, gameOverPanel);
        panels.Add(GameState.GameClear, gameClearPanel);

        pausePanel.SetActive(false);
    }

    private void Update()
    {
        
    }

    public void SetGameTimer(float time)
    {
        textGameTimer.text = $"{(int)time}";
    }

    public void SetPlayerHealth(int health)
    {
        health = Mathf.Clamp(health, 0, heartImages.Count);

        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].enabled = i < health ? true : false;
        }
    }
}
