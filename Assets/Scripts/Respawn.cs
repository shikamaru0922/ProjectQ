using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // ����������ڶ���Ҫ�ƶ�������λ�õ�ƫ����
    public Vector2 respawnOffset = new Vector2(0, 100);

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ����Ƿ�����ҽ����˴���������
        // ������������һ����Ϊ"Player"�ı�ǩ������Ը���ʵ����������޸�
        if (other.CompareTag("Player"))
        {
            Vector2 newPosition = new Vector2(other.transform.position.x, transform.position.y) + respawnOffset;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.transform.position = newPosition;
        }
    }
}
