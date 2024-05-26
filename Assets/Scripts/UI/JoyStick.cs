using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public RectTransform joyStick;
    public RectTransform stick;
    private float radius;

    private bool isDraging = false;

    private Vector2 direction = Vector2.zero;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        radius = joyStick.sizeDelta.x * 0.5f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDraging)
        {
            joyStick.transform.position = Input.mousePosition;
            stick.anchoredPosition = Vector2.zero;
            direction = Vector2.zero;
        }

        if (isDraging)
        {
            Logger.Log($"{direction.x}, {direction.y}");
        }
    }

    public float GetAxis(Axis axis)
    {
        switch (axis)
        {
            case Axis.Horizontal:
                return direction.x;
            case Axis.Vertical:
                return direction.y;
        }

        return 0f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDraging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(joyStick, eventData.position, canvas.worldCamera, out var pos))
        {
            pos = Vector3.ClampMagnitude(pos, radius);
            stick.anchoredPosition = pos;

            direction = pos.normalized;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDraging = false;
        stick.anchoredPosition = Vector2.zero;
    }
}