using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    bool isDragging, isMobilePlatform;
    Vector2 tapPoint, swipeDelta;
    float minSwipeDelta = 100;

    public delegate void OnSwipeInput(MoveTo moveTo);
    public static event OnSwipeInput SwipeEvent;

    public static MoveTo Forward = MoveTo.Forward;
    public static MoveTo Left = MoveTo.Left;
    public static MoveTo Right = MoveTo.Right;

    private void Awake()
    {
        #if UNITY_EDITOR || UNITY_STANDALONE
            isMobilePlatform = false;
        #else
            isMobilePlatform = true;
        #endif
    }

    private void Update()
    {
        if (!isMobilePlatform)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                tapPoint = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
                ResetSwipe();
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    isDragging = true;
                    tapPoint = Input.touches[0].position;
                }
                else if (Input.touches[0].phase == TouchPhase.Canceled ||
                         Input.touches[0].phase == TouchPhase.Ended)
                    ResetSwipe();
            }
        }

        CalculateSwipe();
    }

    void CalculateSwipe()
    {
        swipeDelta = Vector2.zero;

        if (isDragging)
        {
            if (!isMobilePlatform && Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - tapPoint;
            else if (Input.touchCount > 0)
                swipeDelta = Input.touches[0].position - tapPoint;
        }

        if (swipeDelta.magnitude > minSwipeDelta)
        {
            if (SwipeEvent != null)
            {
                if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    SwipeEvent(swipeDelta.x < 0 ? Left : Right);
                else if (swipeDelta.y > 0)
                    SwipeEvent(Forward);
            }

            ResetSwipe();
        }
    }

    void ResetSwipe()
    {
        isDragging = false;
        tapPoint = swipeDelta = Vector2.zero;
    }
}
