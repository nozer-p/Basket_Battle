using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    private Rigidbody rb;
    private new Collider collider;

    private int countCol;
    public float ForceValue;

    private void Start()
    {
        Physics.defaultMaxDepenetrationVelocity = 100.0f;
        Physics.gravity = new Vector3(0, -25, 0);
        countCol = 0;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();

        SwipeDetection.SwipeEvent += OnSwipe;
    }

    private void FixedUpdate()
    {

    }

    private void OnSwipe(Vector2 direction, float delta)
    {
        Vector3 dir = (Vector3)direction;
        Move(dir, delta);
        ResetCountCol();
    }

    private void Move(Vector3 direction, float delta)
    {
        rb.velocity = direction * ForceValue * delta;
        collider.material.bounciness = 0.82f;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.tag == "Walls")
        {
            countCol++;
            if (countCol >= 3 && collider.material.bounciness > 0f)
            {
                collider.material.bounciness /= 1.15f;
            }
            
            if(collider.material.bounciness <= 0.001f)
            {
                ResetCountCol();
            }
        }
    }
    
    private void ResetCountCol()
    {
        countCol = 0;
    }

    public void FreezeRotation(bool log)
    {
        if (rb != null)
        {
            rb.freezeRotation = log;
        }
    }
}
