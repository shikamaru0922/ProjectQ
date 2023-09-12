using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Animator coinAnimator;
    private Rigidbody2D rb;

    public AudioClip coinSound;  // 金币音效
    public float jumpForce = 5f;
    public float animationSpeedMultiplier = 2f;
    public float waitTimeBeforeDestroy = 0.5f; // 等待0.5秒后销毁金币
    public float coinSoundVolume = 0.5f;  // 默认设置为0.5，但你可以在Unity编辑器中进行调整


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
        if (collision.CompareTag("Player")) // 假设玩家的tag是"Player"
        {
            CollectCoin();
        }
    }

    private void CollectCoin()
    {
        coinAnimator.speed *= animationSpeedMultiplier; // 加速动画速度
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse); // 向上跳跃

        PlayCoinSound();  // 播放金币音效

        // 在等待一定的时间后销毁金币
        Destroy(gameObject, waitTimeBeforeDestroy);
    }

    private void PlayCoinSound()
    {
        if (coinSound != null)
        {
            AudioSource.PlayClipAtPoint(coinSound, transform.position, coinSoundVolume);
        }
    }
}
