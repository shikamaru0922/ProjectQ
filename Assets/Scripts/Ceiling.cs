using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(TilemapRenderer))]
public class Ceiling : MonoBehaviour
{
    public Tilemap ceilingTilemap;
    public float pulseDuration = 2f;
    public Color targetColor = Color.red;
    private Color originalColor;

    [Header("Falling Effect")]
    public float fallSpeed = 5.0f;
    private bool shouldFall = false;

    public float riseDistance = 10f;  // 上升的距离
    public float riseDuration = 1f;  // 上升的时间


    private bool isTouchingMagnet = false;
    private Transform magnetTransform; // 用于存储与"Magnet"对象的Transfor
    private void Start()
    {
       // originalColor = ceilingTilemap.color;

        // 使Tilemap的颜色呼吸灯效果
        //DOTween.To(() => ceilingTilemap.color, x => ceilingTilemap.color = x, targetColor, pulseDuration)
        //       .SetLoops(-1, LoopType.Yoyo);
    }

    private void Update()
    {
        if (shouldFall)
        {
            FallDown();
        }
        if (GameManager.Instance.bossIsHit) 
        {
            RiseFast();
            GameManager.Instance.bossIsHit = false;
        }
        

    }

    void FallDown()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime, transform.position.z);

        // 抖动效果
        //transform.DOShakePosition(0.2f, new Vector3(0.1f, 0.02f, 0f), 10, 90, false, true);

        if (GameManager.Instance.bossIsDead)
        {
            shouldFall = false;
        }
    }

    public void RiseFast()
    {
        transform.DOMoveY(transform.position.y + riseDistance, riseDuration);
    }

    // 这是在Ceiling的子物体"FallingTrigger"上触发的方法
    public void StartFalling()
    {
        shouldFall = true;
    }



}