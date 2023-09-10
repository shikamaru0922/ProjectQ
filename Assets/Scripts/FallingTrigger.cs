using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrigger : MonoBehaviour
{
    private Ceiling parentCeiling;
    public LayerMask playerLayer;   // ���ڱ�ʶ��ҵ�Layer

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
        }
    }
}
