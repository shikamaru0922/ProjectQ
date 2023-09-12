using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrigger : MonoBehaviour
{
    private Ceiling parentCeiling;
    public LayerMask playerLayer;   // ���ڱ�ʶ��ҵ�Layer
    public GameObject wall_1;
    public GameObject wall_2;

    public float dropTime = 2f;  // ���Ǵӵ�ǰλ���ƶ���Ŀ��λ�������ʱ��
    private Vector3 start1, start2, end1, end2;

    private void Start()
    {
        parentCeiling = GetComponentInParent<Ceiling>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ����ҽ���Triggerʱ��ʼ��׹
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
        // ����ֻ�轻����ʼ�ͽ�����λ�������ص�ԭʼλ��
        StartCoroutine(MoveObjects(end1, start1, wall_1));
        StartCoroutine(MoveObjects(end2, start2, wall_2));
    }
    public void DropObjects()
    {
        // ���ÿ�ʼ�ͽ�����λ��
        start1 = wall_1.transform.position;
        start2 = wall_2.transform.position;
        end1 = start1 + Vector3.down * 3;  // �����ƶ�������λ
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

            float fraction = t / dropTime;  // �����ƶ��ı���

            obj.transform.position = Vector3.Lerp(fromPosition, toPosition, fraction);

            yield return null;
        }

        // ȷ��������ȫ�ƶ���Ŀ��λ��
        obj.transform.position = toPosition;
    }

 
}
