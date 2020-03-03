﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;

    public float smoothSpeed = 10f;
    public Vector3 offset;
    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPostion = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPostion;
    }
}
