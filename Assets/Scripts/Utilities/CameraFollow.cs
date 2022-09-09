using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float cameraOffsetX;
    public float cameraOffsetY;

    bool startingZoom = true;

    private void LateUpdate()
    {
        if (startingZoom)
        {

        }

        transform.position = new Vector3(target.position.x + cameraOffsetX, target.position.y + cameraOffsetY, transform.position.z);
    }
}
