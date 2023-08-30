using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetTrigger : MonoBehaviour
{
    public GameObject targetObject;
    List<GameObject> magnetObjects = new List<GameObject>();
    private void Start()
    {

    }

    private void Update()
    {
        UpdateClosestTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.GetComponent<MagnetInEnviroment>() != null)
        {
            if (!magnetObjects.Contains(other.gameObject))
            {
                magnetObjects.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<MagnetInEnviroment>() != null)
        {
            magnetObjects.Remove(other.gameObject);
        }
    }

    void UpdateClosestTarget()
    {
        float minDistance = float.MaxValue;
        GameObject closestObject = null;

        foreach (var magnet in magnetObjects)
        {
            float distance = Vector2.Distance(transform.position, magnet.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestObject = magnet;
            }
        }

        targetObject = closestObject;
    }
}
