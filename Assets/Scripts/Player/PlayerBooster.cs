using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBooster : MonoBehaviour
{
    // 부스터 지속 시간, 부스터 스피드(스케일), 부스터 충돌 범위
    private float boosterDuration;
    private float nextBoosterEndTime;
    private float boosterSpeed;
    private float boosterColliderRange;

    public Button pauseButton;

    private void OnEnable()
    {
        // 타이머 시작
        nextBoosterEndTime = Time.time + boosterDuration;
        Time.timeScale = boosterSpeed;

        pauseButton.enabled = false;
    }

    private void Update()
    {
        if (nextBoosterEndTime <= Time.time)
        {
            Time.timeScale = 1f;
            pauseButton.enabled = true;
        }
    }
}
