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
        if (other.tag == "Player") 
        {   //if(other.GetComponent<Collision>().onMagnet == true && other.GetComponent<MagneticPlayer>().isAtrracting)
            Debug.Log(1213123);
            //other.transform.position = Vector3.MoveTowards(other.transform.position,endPoint.position, speed * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
