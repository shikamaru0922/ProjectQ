using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MagneticPlayer;

public class MagnetInEnviroment : MonoBehaviour
{
    public PoleType pole;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        var horizontalMoveComponent = gameObject.GetComponent<HorizontalMove>();

        if (collision.transform.tag == "Player" && horizontalMoveComponent != null && !horizontalMoveComponent.enabled)
        {
            horizontalMoveComponent.enabled = true;
        }
    }
}
