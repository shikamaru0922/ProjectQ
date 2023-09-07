using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator coinAnimator;
    private Rigidbody2D rb;
    public float jumpForce = 5f;
    public float animationSpeedMultiplier = 2f;
    public float waitTimeBeforeDestroy = 0.5f; // �ȴ�0.5������ٽ��

    private void Start()
    {
        coinAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        if (coinAnimator == null || rb == null)
        {
            Debug.LogError("Required components are missing on the coin!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // ������ҵ�tag��"Player"
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        coinAnimator.speed *= animationSpeedMultiplier; // ���ٶ����ٶ�
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // ������Ծ

        // �ڵȴ�һ����ʱ������ٽ��
        Destroy(gameObject, waitTimeBeforeDestroy);
    }
}
