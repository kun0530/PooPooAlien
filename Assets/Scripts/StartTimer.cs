using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    public List<Image> timerImages;
    private int timerIndex;

    private float timerInterval = 1f;
    private float nextCountTime;
    private float prevTimeScale;

    private bool isActiveTimer;

    public PlayerMovement playerMovement;

    private void Awake()
    {
        isActiveTimer = false;
    }

    private void Start()
    {
        foreach (var image in timerImages)
        {
            image.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!isActiveTimer)
            return;

        if (Time.unscaledTime >= nextCountTime)
        {
            nextCountTime = Time.unscaledTime + timerInterval;
            if (timerIndex < timerImages.Count && timerIndex >= 0)
                timerImages[timerIndex].gameObject.SetActive(false);
            timerIndex--;
            if (timerIndex < timerImages.Count && timerIndex >= 0)
                timerImages[timerIndex].gameObject.SetActive(true);
        }

        if (timerIndex < 0)
        {
            isActiveTimer = false;
            Time.timeScale = prevTimeScale;
            playerMovement.enabled = true;
        }
    }

    public void ActiveTimer()
    {
        isActiveTimer = true;

        timerIndex = timerImages.Count;
        nextCountTime = 0f;

        prevTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        playerMovement.enabled = false;
    }
}
