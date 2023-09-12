using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damagePerSecond = 10.0f; // 每秒扣除的血量，可以在Unity编辑器中调整
    private bool isPlayerInLaserRange = false; // 玩家是否在激光范围内

    private PlayerState playerState; // 玩家的状态类引用


    public AudioClip attackSound;
    public float attackSoundVolume = 0.5f;
    private void Update()
    {
        // 如果玩家在激光范围内，则扣血
        if (isPlayerInLaserRange && playerState != null)
        {
            playerState.currentHealth -= damagePerSecond * Time.deltaTime;
        }
    }

    public void SetTriggerTure() 
    {
        GetComponent<BoxCollider2D>().enabled = true;
        PlayShootingSound();


    }

    public void SetTriggerFalse()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        gameObject.SetActive(false);

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰撞体是玩家
        if (other.CompareTag("Player"))
        {
            playerState = other.gameObject.GetComponent<PlayerState>();
            if (playerState != null)
            {
                isPlayerInLaserRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 如果玩家离开激光范围
        if (other.CompareTag("Player"))
        {
            isPlayerInLaserRange = false;
            playerState = null;
        }
    }

    private void PlayShootingSound()
    {
        if (attackSound != null)
        {
            AudioSource.PlayClipAtPoint(attackSound, transform.position, attackSoundVolume);
        }
    }
}
