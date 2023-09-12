using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrigger : MonoBehaviour
{
    private Ceiling parentCeiling;
    public LayerMask playerLayer;   // 用于标识玩家的Layer
    public GameObject wall_1;
    public GameObject wall_2;

    public float dropTime = 2f;  // 这是从当前位置移动到目标位置所需的时间
    private Vector3 start1, start2, end1, end2;

    private void Start()
    {
        parentCeiling = GetComponentInParent<Ceiling>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 当玩家进入Trigger时开始下坠
        if (playerLayer == (playerLayer | (1 << other.gameObject.layer)))
        {
            parentCeiling.StartFalling();
            DropObjects();
        }

        if (GameManager.Instance.bossIsDead) 
        {
            ReturnToOriginalPosition();
        }
    }


    public void ReturnToOriginalPosition()
    {
        // 我们只需交换开始和结束的位置来返回到原始位置
        StartCoroutine(MoveObjects(end1, start1, wall_1));
        StartCoroutine(MoveObjects(end2, start2, wall_2));
    }
    public void DropObjects()
    {
        // 设置开始和结束的位置
        start1 = wall_1.transform.position;
        start2 = wall_2.transform.position;
        end1 = start1 + Vector3.down * 3;  // 向下移动三个单位
        end2 = start2 + Vector3.down * 3;

        StartCoroutine(MoveObjects(start1, end1, wall_1));
        StartCoroutine(MoveObjects(start2, end2, wall_2));
    }

    IEnumerator MoveObjects(Vector3 fromPosition, Vector3 toPosition, GameObject obj)
    {
        float t = 0;

        while (t < dropTime)
        {
            t += Time.deltaTime;

            float fraction = t / dropTime;  // 计算移动的比例

            obj.transform.position = Vector3.Lerp(fromPosition, toPosition, fraction);

            yield return null;
        }

        // 确保物体完全移动到目标位置
        obj.transform.position = toPosition;
    }

 
}
