using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{

    private float originalTimeScale = 1.0f;
    [SerializeField] private float slowdownFactor = 0.5f;  // ���������Inspector�����ü��������Ϸ�ٶ�

    // ������������봥������ʱ����
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            Time.timeScale = slowdownFactor;
        }
    }

    // �����������뿪��������ʱ����
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(11);
            Time.timeScale = 1;
        }
    }
}