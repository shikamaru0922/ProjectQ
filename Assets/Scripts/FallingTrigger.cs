using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingTrigger : MonoBehaviour
{
    private Ceiling parentCeiling;
    public LayerMask playerLayer;   // 用于标识玩家的Layer

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
        }
    }
}
