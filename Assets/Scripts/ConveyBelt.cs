using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyBelt : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject belt;
    public Transform endPoint;
    public float speed;
    void Start()
    {
        belt = this.gameObject;

    }

    private void OnTriggerStay(Collider other)
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
