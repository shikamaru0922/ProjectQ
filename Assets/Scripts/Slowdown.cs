using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slowdown : MonoBehaviour
{

    private float originalTimeScale = 1.0f;
    [SerializeField] private float slowdownFactor = 0.5f;  // 这里可以在Inspector中设置减慢后的游戏速度

    // 当其他对象进入触发区域时调用
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
           
            Time.timeScale = slowdownFactor;
        }
    }

    // 当其他对象离开触发区域时调用
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(11);
            Time.timeScale = 1;
        }
    }
}