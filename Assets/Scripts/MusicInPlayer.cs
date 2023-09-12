using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInPlayer : MonoBehaviour
{
    public AudioClip playerDeathSound;    // ���������Ч
    public AudioClip playerRepelSound;    // ����ų���Ч
    public AudioClip playerAttractSound;    // ����ų���Ч
    public float volume = 1.0f;           // Ĭ������Ϊ1.0���������Unity�༭���е���
    public float deathvolume = 1.0f;
    private AudioSource audioSource;
    public float attractVolume;

    public bool isAttractSoundPlaying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // ���û���ҵ�AudioSource��������һ��
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void PlayDeathSound()
    {
        if (playerDeathSound != null)
        {
            audioSource.PlayOneShot(playerDeathSound, deathvolume);
        }
    }

    public void PlayAttractSound()
    {
        if (playerAttractSound != null)
        {
            audioSource.clip = playerAttractSound;
            audioSource.volume = attractVolume;
            audioSource.loop = true; // �趨Ϊѭ������
            audioSource.Play();
        }
    }

    public void StopAttractSound()
    {
        if (audioSource.clip == playerAttractSound)
        {
            audioSource.Stop();
        }
    }

    public void PlayRepelSound()
    {
        if (playerRepelSound != null)
        {
            audioSource.PlayOneShot(playerRepelSound, volume);
        }
    }
}
