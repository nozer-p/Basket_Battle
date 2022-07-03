using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    public float ForceValue;

    private Rigidbody rb;
    private new Collider collider;

    private float timeBtwBounce;
    public float startTimeBtwBounce;
    public float smallerBounce;

    private void Start()
    {
        Physics.gravity = new Vector3(0, -20, 0);

        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void OnSwipe(Vector2 direction, float delta)
    {
        Vector3 dir = (Vector3)direction;

        Move(dir, delta);
    }

    private void Update()
    {
        if (timeBtwBounce <= 0)
        {
            if (collider.material.bounciness > 0.001)
            {
                collider.material.bounciness /= smallerBounce;
            }

            timeBtwBounce = startTimeBtwBounce;
        }
        else
        {
            timeBtwBounce -= Time.deltaTime;
        }
    }

    private void Move(Vector3 direction, float delta)
    {
        rb.velocity = direction * ForceValue * delta;
        collider.material.bounciness = 1;
    }
}
