using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;

public class StageUiManager : MonoBehaviour
{
    public TextMeshProUGUI textGameTimer;
    public List<Image> heartImages;

    public GameObject pausePanel;

    private void Start()
    {
        pausePanel.SetActive(false);
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
        pausePanel.SetActive(isActive);
        Time.timeScale = isActive ? 0f : 1f;
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
