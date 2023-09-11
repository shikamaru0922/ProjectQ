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

    public float riseDistance = 10f;  // �����ľ���
    public float riseDuration = 1f;  // ������ʱ��


    private bool isTouchingMagnet = false;
    private Transform magnetTransform; // ���ڴ洢��"Magnet"�����Transfor
    private void Start()
    {
       // originalColor = ceilingTilemap.color;

        // ʹTilemap����ɫ������Ч��
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

        // ����Ч��
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

    // ������Ceiling��������"FallingTrigger"�ϴ����ķ���
    public void StartFalling()
    {
        shouldFall = true;
    }



}