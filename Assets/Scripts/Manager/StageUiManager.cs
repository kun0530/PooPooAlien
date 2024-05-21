using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;

public class StageUiManager : MonoBehaviour
{
    private GameManager gameManager;

    public TextMeshProUGUI textGameTimer;
    public List<Image> heartImages;

    public GameObject pausePanel;
    private float prevTimeScale;

    private void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        pausePanel.SetActive(false);
        prevTimeScale = 1f;
    }

    private void Update()
    {
        
    }

    public void SetGameTimer(float time)
    {
        textGameTimer.text = $"{time}";
    }

    public void ActivePausePanel(bool isActive)
    {
        if (isActive)
        {
            prevTimeScale = Time.timeScale;
        }
        pausePanel.SetActive(isActive);
        Time.timeScale = isActive ? 0f : prevTimeScale;
        gameManager.gameStatus = isActive ? GameStatus.Pause : GameStatus.Running;
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
