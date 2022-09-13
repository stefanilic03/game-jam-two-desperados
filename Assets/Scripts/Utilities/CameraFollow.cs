using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    public float cameraOffsetX;
    public float cameraOffsetY;

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.position.x + cameraOffsetX, /*target.position.y +*/ cameraOffsetY, transform.position.z);
        }
    }
}
