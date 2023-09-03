using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skull : MonoBehaviour
{
    public Animator skull_anim;
    public Enemy skull_enemy;
    public LayerMask playerLayer;
    public float bounceForce;
    // Start is called before the first frame update
    private void Awake()
    {
        skull_enemy = gameObject.GetComponentInParent<Enemy>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)  // 检查是否与玩家层碰撞。
        {
            skull_enemy.TakeDamage(1);  // 敌人扣血。
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce); // 给玩家一个向上的弹力。
            }

        }
        
    }
}
