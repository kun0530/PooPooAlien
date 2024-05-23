using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    public float visibleTime = 1f;
    public float invisibleTime = 1f;
    private float timer;
    private bool isVisible;
    private bool IsVisible{
        get { return isVisible; }
        set {
            if (blinkImage == null)
                return;

            isVisible = value;
            blinkImage.enabled = isVisible;
            timer = 0f;
        }
    }

    private Image blinkImage;

    private void Awake()
    {
        blinkImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        IsVisible = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (IsVisible && timer >= visibleTime)
        {
            IsVisible = false;
        }

        if (!IsVisible && timer >= invisibleTime)
        {
            IsVisible = true;
        }
    }
}
