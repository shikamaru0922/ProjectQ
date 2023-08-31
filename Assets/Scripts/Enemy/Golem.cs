using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem :Enemy
{
    public GameObject laser;
    // Start is called before the first frame update
/*    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
    }*/

    public override void Move()
    {
        base.Move();
        
    }

    public override void DetectPlayer()
    {
        base.DetectPlayer();
        if(findPlayer == false)
        anim.SetTrigger("Chase");

        //ShootLaser();
    }

    //这是一个错误
    public void ShootLaser() 
    {
        laser.GetComponent<Animator>().Play("laser_Clip", 0);
    }


}
