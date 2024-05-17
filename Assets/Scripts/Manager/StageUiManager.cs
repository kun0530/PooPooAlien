using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Numerics;

public class StageUiManager : MonoBehaviour
{
    public RectTransform safeArea;

    public TextMeshProUGUI textGameTimer;
    public List<Image> heartImages;

    public GameObject pausePanel;

    private void Start()
    {
        var minAnchor = Screen.safeArea.min;
        var maxAnchor = Screen.safeArea.max;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        safeArea.anchorMin = minAnchor;
        safeArea.anchorMax = maxAnchor;

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
