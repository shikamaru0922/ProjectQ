using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip musicClip;  // 拖拽你的音乐文件到这里

    private AudioSource audioSource;

    // 在Awake中配置，以确保音乐在游戏开始时播放
    void Awake()
    {
        // 获取AudioSource组件
        audioSource = GetComponent<AudioSource>();

        // 设置音乐和循环
        audioSource.clip = musicClip;
        audioSource.loop = true;  // 设置为循环播放

        // 播放音乐
        audioSource.Play();

        // 确保在加载新的场景时，这个对象不会被销毁
        DontDestroyOnLoad(gameObject);
    }
}
