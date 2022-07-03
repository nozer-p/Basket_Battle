using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    public static event OnSwipeInput SwipeEvent;
    public delegate void OnSwipeInput(Vector2 direction, float delta);

    private Vector2 tapPos;
    private Vector2 tapPosOld;
    private Vector2 tapPosNow;
    private Vector2 swipeDelta;
    private Vector2 swipeDeltaOld;

    private float minDeadZone = 100;
    private float maxDeadZone = 500;
    private float delta = 0;

    private bool isSwiping;
    private bool isMobile;

    public SlowMo slowMo;

    public BallMovement ball;

    public GameObject arrow;
    public float offset;

    private void Start()
    {
        isMobile = Application.isMobilePlatform;
        VisibleArrow(false);
    }

    private void Update()
    {
        if (!isMobile)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSwiping = true;
                VisibleArrow(true);
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
                    VisibleArrow(true);
                    tapPos = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended)
                {
                    CheckSwipe();
                }
            }
        }

        if (isSwiping)
        {
            tapPosNow = Input.mousePosition;

            if (tapPos == tapPosNow)
            {   
                Vector3 direction = -Input.mousePosition + (Vector3)tapPosOld;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
            }
            else
            {
                Vector3 direction = -Input.mousePosition + (Vector3)tapPos;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrow.transform.rotation = Quaternion.Euler(0f, 0f, angle + offset);
                tapPosOld = tapPos;
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

        if (swipeDelta.magnitude < minDeadZone)
        {
            delta = minDeadZone;
        }
        if (swipeDelta.magnitude > maxDeadZone)
        {
            delta = maxDeadZone;
        }
        if (swipeDelta.magnitude > minDeadZone && swipeDelta.magnitude < maxDeadZone)
        {
            delta = swipeDelta.magnitude;
        }

        if (SwipeEvent != null && delta != 0f)
        {
            if (swipeDelta.normalized == new Vector2(0f, 0f))
            {
                if (swipeDeltaOld == new Vector2(0f, 0f))
                {
                    swipeDeltaOld = tapPos;
                    SwipeEvent(swipeDeltaOld.normalized, delta);
                }
                else
                {
                    SwipeEvent(swipeDeltaOld.normalized, delta);
                }
            }
            else
            {
                swipeDeltaOld = swipeDelta;
                SwipeEvent(swipeDelta.normalized, delta);
            }
        }

        ResetSwipe();
    }

    private void ResetSwipe()
    {
        isSwiping = false;
        delta = 0f;
        VisibleArrow(false);
        tapPos = Vector2.zero;
        swipeDelta = Vector2.zero;
    }

    private void VisibleArrow(bool log)
    {
        if (slowMo != null && ball != null && log)
        {
            slowMo.SlowMotion(true);
            ball.FreezeRotation(true);
            arrow.SetActive(true);
        }
        else if (slowMo != null && ball != null && !log)
        {
            slowMo.SlowMotion(false);
            ball.FreezeRotation(false);
            arrow.SetActive(false);
        }
    }
}