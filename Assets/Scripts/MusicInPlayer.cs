using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicInPlayer : MonoBehaviour
{
    public AudioClip playerDeathSound;    // 玩家死亡音效
    public AudioClip playerRepelSound;    // 玩家排斥音效
    public AudioClip playerAttractSound;    // 玩家排斥音效
    public float volume = 1.0f;           // 默认音量为1.0，你可以在Unity编辑器中调整
    public float deathvolume = 1.0f;
    private AudioSource audioSource;
    public float attractVolume;

    public bool isAttractSoundPlaying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            // 如果没有找到AudioSource组件，添加一个
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
            audioSource.loop = true; // 设定为循环播放
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
