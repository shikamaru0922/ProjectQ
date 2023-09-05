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
    private void OnCollisionExit2D(Collision2D collision)
    {
       /* if (collision.gameObject.tag == "Player") 
        {
            Debug.Log("123");
            if (GetComponent<Rigidbody2D>().isKinematic) 
            {   
                if(collision.gameObject.GetComponent<MagneticPlayer>().currentPole == this.pole)
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            }
        }*/
    }
}
