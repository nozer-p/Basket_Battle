using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction, float delta);

    private Vector2 tapPos;
    private Vector2 swipeDelta;

    private float minDeadZone = 50;
    private float maxDeadZone = 500;
    private float delta = 0;

    private bool isSwiping;
    private bool isMobile;

    private void Start()
    {
        isMobile = Application.isMobilePlatform;
    }

    private void Update()
    {
        if (!isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                tapPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                CheckSwipe();
            }
        }
        else
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    isSwiping = true;
                    tapPos = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    CheckSwipe();
                }
            }
        }
    }

    private void CheckSwipe()
    {
        swipeDelta = Vector2.zero;

        if (isSwiping)
        {
            if (!isMobile && Input.GetMouseButtonUp(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - tapPos;
            }
            else if (Input.touchCount > 0)
            {
                swipeDelta = Input.GetTouch(0).position - tapPos;
            }
        }

        if (swipeDelta.magnitude > minDeadZone)
        {
            if (swipeDelta.magnitude > maxDeadZone)
            {
                delta = maxDeadZone;
            }
            else
            {
                delta = swipeDelta.magnitude;
            }

            if (SwipeEvent != null)
            {
                SwipeEvent(swipeDelta.normalized, delta);
            }

            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        isSwiping = false;

        tapPos = Vector2.zero;
        swipeDelta = Vector2.zero;
    }
}