using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float damagePerSecond = 10.0f; // ÿ��۳���Ѫ����������Unity�༭���е���
    private bool isPlayerInLaserRange = false; // ����Ƿ��ڼ��ⷶΧ��

    private PlayerState playerState; // ��ҵ�״̬������


    public AudioClip attackSound;
    public float attackSoundVolume = 0.5f;
    private void Update()
    {
        // �������ڼ��ⷶΧ�ڣ����Ѫ
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
        // �����ײ�������
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
        // �������뿪���ⷶΧ
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
