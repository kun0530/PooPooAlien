using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiNumberIncrease : MonoBehaviour
{
    private TextMeshProUGUI NumberText;
    public string numberFormat = "{0}";

    private float targetNum;
    public float TargetNum {
        get { return targetNum; }
        set {
            if (NumberText == null)
                return;

            if (increaseCount <= 0 || increaseInterval <= 0f)
            {
                CurrentNum = value;
                return;
            }

            targetNum = value;

            increaseNum = (targetNum - currentNum) / increaseCount;
            currentCount = 0;

            nextIncreaseTime = Time.time + increaseInterval;

            isIncreasing = true;
        }
    }
    private float currentNum;
    public float CurrentNum{
        get { return currentNum; }
        set {
            if (NumberText == null)
                return;

            currentNum = value;
            NumberText.text = string.Format(numberFormat, (int)currentNum);
        }
    }

    public float increaseDuration;
    public int increaseCount;
    private float increaseInterval;

    private float increaseNum;
    private int currentCount;
    private float nextIncreaseTime;

    private bool isIncreasing;

    private void Awake()
    {
        NumberText = GetComponent<TextMeshProUGUI>();
        if (NumberText == null)
            enabled = false;

        increaseInterval = (increaseCount > 0 && increaseDuration > 0f) ? increaseDuration / increaseCount : 0f;
        isIncreasing = false;
    }

    private void Update()
    {
        if (isIncreasing && nextIncreaseTime <= Time.time)
        {
            currentCount++;
            
            if (currentCount < increaseCount)
            {
                currentNum += increaseNum;
                nextIncreaseTime = Time.time + increaseInterval;
            }
            else
            {
                currentNum = targetNum;
                isIncreasing = false;
            }
            NumberText.text = string.Format(numberFormat, (int)currentNum);
        }
    }
}
