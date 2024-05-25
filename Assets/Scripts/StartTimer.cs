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
    private float prevTimeScale;

    public Button pauseButton;
    public PlayerMovement playerMovement;

    private void Awake()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        prevTimeScale = 1f;
    }

    private void OnEnable()
    {
        timer = 0f;
        currentTimer = startTimer;
        timerText.text = $"{currentTimer}";
        prevTimeScale = Time.timeScale;
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
            Time.timeScale = prevTimeScale;

            pauseButton.enabled = true;
            playerMovement.enabled = true;

            gameObject.SetActive(false);
        }
    }
}
