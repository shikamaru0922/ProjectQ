using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    public AudioClip musicClip;  // ��ק��������ļ�������

    private AudioSource audioSource;

    // ��Awake�����ã���ȷ����������Ϸ��ʼʱ����
    void Awake()
    {
        // ��ȡAudioSource���
        audioSource = GetComponent<AudioSource>();

        // �������ֺ�ѭ��
        audioSource.clip = musicClip;
        audioSource.loop = true;  // ����Ϊѭ������

        // ��������
        audioSource.Play();

        // ȷ���ڼ����µĳ���ʱ��������󲻻ᱻ����
        DontDestroyOnLoad(gameObject);
    }
}
