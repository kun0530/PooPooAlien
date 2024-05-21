using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    public float startTimer = 3f;
    private float timerInterval = 1f;
    private float timer = 0f;
    private float currentTimer;

    public Button pauseButton;
    public PlayerMovement playerMovement;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        timer = 0f;
        currentTimer = startTimer + 1f;
        timerText.text = $"{currentTimer}";
        Time.timeScale = 0f;

        pauseButton.enabled = false;
        playerMovement.enabled = false;
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;
        if (timer >= timerInterval)
        {
            currentTimer--;
            if (currentTimer <= 0f)
            {
                timerText.text = "Start!";
            }
            else
            {
                timerText.text = $"{currentTimer}";
            }
            timer = 0f;
        }

        if (currentTimer < 0f)
        {
            Time.timeScale = 1f;

            pauseButton.enabled = true;
            playerMovement.enabled = true;

            this.gameObject.SetActive(false);
        }
    }
}
