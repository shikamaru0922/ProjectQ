using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // 这个变量用于定义要移动到的新位置的偏移量
    public Vector2 respawnOffset = new Vector2(0, 100);

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 检查是否是玩家进入了触发器区域
        // 这里假设玩家有一个名为"Player"的标签，你可以根据实际情况进行修改
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerIsDead = true;
            other.GetComponent<MusicInPlayer>().PlayDeathSound();
        }
    }
}
