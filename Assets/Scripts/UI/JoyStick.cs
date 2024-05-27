using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public enum Axis
    {
        Horizontal,
        Vertical,
    }

    public RectTransform joyStick;
    public RectTransform stick;
    private RectTransform joyStickRange;
    private float radius;

    private bool isDraging = false;

    private Vector2 direction = Vector2.zero;

    private Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        joyStickRange = GetComponent<RectTransform>();
        radius = joyStick.sizeDelta.x * 0.5f;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isDraging)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(joyStickRange, Input.mousePosition, canvas.worldCamera))
            {
                isDraging = true;

                joyStick.transform.position = Input.mousePosition;
                stick.anchoredPosition = Vector2.zero;
                direction = Vector2.zero;
            }
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
        direction = Vector2.zero;
    }
}