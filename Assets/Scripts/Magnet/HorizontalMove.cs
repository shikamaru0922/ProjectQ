using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalMove : MonoBehaviour
{
    public float speed = 5f;
    public float distance = 10f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float lerpTime;
    private bool isReturning = false;

    private void Start()
    {
        startPosition = transform.position;
        endPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
        lerpTime = 0;
    }

    private void Update()
    {
        lerpTime += Time.deltaTime * speed;

        if (!isReturning)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpTime);
            if (lerpTime >= 1)
            {
                isReturning = true;
                lerpTime = 0;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(endPosition, startPosition, lerpTime);
            if (lerpTime >= 1)
            {
                isReturning = false;
                lerpTime = 0;
            }
        }
    }
}
