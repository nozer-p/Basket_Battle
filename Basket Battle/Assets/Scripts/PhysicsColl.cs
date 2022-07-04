using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsColl : MonoBehaviour
{
    private void Start()
    {
        Physics.defaultMaxDepenetrationVelocity = 100.0f;
        Physics.gravity = new Vector3(0, -25, 0);
    }
}