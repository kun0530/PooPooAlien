using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public enum Directions
{
    None = -1,
    Up,
    Down,
    Left,
    Right,
}

public class TouchManager : Singleton<TouchManager>
{
    public bool Tap { get; private set; }
    public bool LongTap { get; private set; }
    public bool DoubleTap { get; private set; }
    public Directions Swipe { get; private set; }
    public float PinchZoom { get; private set; } = 1f;
    public float RotateAngle { get; private set; } = 0f;

    private int primaryFingerId = int.MinValue;
    private int SecondaryFingerId = int.MinValue;
    
    private float timeTap = 0.25f;
    private float timeLongTap = 0.5f;
    private float timeDoubleTap = 0.25f;
    private float timeSwipe = 0.5f;

    private float primaryStartTime = 0f;

    private bool isFirstTap = false;
    private float firstTapTime = 0f;

    private Vector2 primaryTouchStartPos;
    private Vector2 secondrayTouchStartPos;
    private Vector2 primatryTouchCurrentPos;
    private Vector2 secondrayTouchCurrentPos;
    public float minSwipeDistanceInch = 0.25f;
    private float minSwipeDistancePixels;

    private float startAngle;

    private void Start()
    {
        minSwipeDistancePixels = Screen.dpi * minSwipeDistanceInch;
    }

    private void Update()
    {
        Tap = false;
        LongTap = false;
        DoubleTap = false;
        Swipe = Directions.None;
        PinchZoom = 1f;
        RotateAngle = 0f;

        for (int i = 0; i < Input.touchCount; i++)
        {
            var touch = Input.GetTouch(i);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (primaryFingerId == int.MinValue)
                    {
                        primaryFingerId = touch.fingerId;
                        primaryTouchStartPos = touch.position;
                        primatryTouchCurrentPos = primaryTouchStartPos;
                        primaryStartTime = Time.time;
                    }
                    else if (SecondaryFingerId == int.MinValue)
                    {
                        SecondaryFingerId = touch.fingerId;
                        secondrayTouchStartPos = touch.position;
                        secondrayTouchCurrentPos = touch.position;
                        startAngle = Vector2.Angle(secondrayTouchStartPos, primaryTouchStartPos);
                    }
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    if (SecondaryFingerId != int.MinValue)
                    {
                        if (primaryFingerId == touch.fingerId)
                        {
                            primatryTouchCurrentPos = touch.position;
                        }
                        else if (SecondaryFingerId == touch.fingerId)
                        {
                            secondrayTouchCurrentPos = touch.position;
                        }
                        PinchZoom = Vector2.Distance(primaryTouchStartPos, secondrayTouchStartPos) / Vector2.Distance(primatryTouchCurrentPos, secondrayTouchCurrentPos);
                        RotateAngle = Vector2.Angle(secondrayTouchCurrentPos, primatryTouchCurrentPos) - startAngle;
                    }
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (primaryFingerId == touch.fingerId)
                    {
                        primaryFingerId = int.MinValue;
                        SecondaryFingerId = int.MinValue;
                        var delta = touch.position - primaryTouchStartPos;
                        var duration = Time.time - primaryStartTime;

                        if (duration < timeSwipe && delta.magnitude > minSwipeDistancePixels)
                        {
                            Swipe = ConvertNearDirection(delta);
                        }

                        if (duration < timeTap)
                        {
                            Tap = true;

                            if (isFirstTap && Time.time - firstTapTime > timeDoubleTap)
                            {
                                isFirstTap = false;
                            }

                            if (!isFirstTap)
                            {
                                isFirstTap = true;
                                firstTapTime = Time.time;
                            }
                            else if (Time.time - firstTapTime < timeDoubleTap)
                            {
                                DoubleTap = true;
                                isFirstTap = false;
                                firstTapTime = 0f;
                            }
                            else
                            {
                                isFirstTap = false;
                                firstTapTime = 0f;
                            }
                        }

                        if (duration > timeLongTap)
                        {
                            LongTap = true;
                        }
                    }
                    break;
            }
        }
    }

    private Directions ConvertNearDirection(Vector2 dir)
    {
        return Mathf.Abs(dir.x) > Mathf.Abs(dir.y) ?
            (dir.x > 0 ? Directions.Right : Directions.Left) :
            (dir.y > 0 ? Directions.Up : Directions.Down);
    }
}
