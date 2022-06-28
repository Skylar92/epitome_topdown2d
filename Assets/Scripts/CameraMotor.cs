using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;

    public float boundX = 0.15f;
    public float boundY = 0.05f;

    public void Start()
    {
        lookAt = GameManager.Instance.player.transform;
    }

    private void LateUpdate()
    {
        var lookAtPosition = lookAt.position;
        var cameraPosition = transform.position;
        
        var deltaX = CalculatePosition(lookAtPosition.x, cameraPosition.x, boundX);
        var deltaY = CalculatePosition(lookAtPosition.y, cameraPosition.y, boundY);

        cameraPosition += new Vector3(deltaX, deltaY, 0);
        transform.position = cameraPosition;
    }

    private static float CalculatePosition(float lookAtPosition, float transformPosition, float bound)
    {
        var delta = lookAtPosition - transformPosition;
        if (delta > bound || delta < -bound)
        {
            return transformPosition < lookAtPosition ? (delta - bound) : (delta + bound);
        }

        return 0f;
    }
}