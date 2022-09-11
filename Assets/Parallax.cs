using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    float length;
    float startingPosition;
    public GameObject mainCameraObject;

    public float parallaxEffect;

    float temp;
    float distance;

    private void Start()
    {
        startingPosition = transform.position.x;
        length = this.GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        temp = (mainCameraObject.transform.position.x * (1 - parallaxEffect));
        distance = (mainCameraObject.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startingPosition + distance, mainCameraObject.transform.position.y, transform.position.z);

        if (temp + length / 2 > startingPosition + length)
        {
            startingPosition += length;
        }

        return;
    }
}
