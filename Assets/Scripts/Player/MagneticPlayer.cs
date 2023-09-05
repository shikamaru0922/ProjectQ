using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagneticPlayer : MonoBehaviour
{
    public enum PoleType { N_Pole, S_Pole }
    public enum MagneticMode { Normal, Attraction, Repulsion }
    private Rigidbody2D rb;

    [SerializeField]
    public PoleType currentPole;
    private MagneticMode currentMode;

    // 按下时间（用于判断长按和短按）
    private float pressTime = 0f;

    // 判断玩家是否正在按下按键
    private bool isPressing = false;

    public float repulseForce = 300f;  // 这可以是任意你希望的力量值
    public float attractionForce = 5f;

    // 判断短按和长按的时间阈值
    public float longPressThreshold = 0.24f;

    
    private Collision collision;

    public bool isAtrracting;
    public float upForceInRepulse;
    public LayerMask magnetLayer;  // 设置这个LayerMask来包括磁铁的层
    public float detectionRadius = 1f;  // 设置检测半径
    [SerializeField]
    private float originGravity;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collision = GetComponent<Collision>();
        originGravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TogglePole();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            isPressing = true;
            pressTime = 0f;
        }

        if (isPressing)
        {
            pressTime += Time.deltaTime;

            if (pressTime > longPressThreshold)
            {
                SetMagneticMode(MagneticMode.Attraction);
                
                Attract();
            }
        }

        if (Input.GetKeyUp(KeyCode.J))
        {
            isPressing = false;
            GetComponent<Rigidbody2D>().gravityScale = originGravity;
            isAtrracting = false;
            if (pressTime <= longPressThreshold)
            {
                SetMagneticMode(MagneticMode.Repulsion);
                
                Repulse();
            }
        }

    }

    private void TogglePole()
    {
        currentPole = (currentPole == PoleType.N_Pole) ? PoleType.S_Pole : PoleType.N_Pole;
    }

    private void SetMagneticMode(MagneticMode mode)
    {
        currentMode = mode;
    }

    private void Attract()
    {
        if (collision.targetObject != null)
        {

            GameObject target = collision.targetObject;
            // 如果那个物体有MagneticObject脚本（或某种可以确定其属性的脚本）
            MagnetInEnviroment magneticObject = target.GetComponent<MagnetInEnviroment>();
            if (magneticObject != null)
            {
                // 检查物体的极性是否与玩家不同
                if (magneticObject.pole != currentPole)
                {
                    Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        if (rb.isKinematic) // 如果物体是kinematic
                        {
                            // 吸引玩家到物体位置
                            if (collision.isInX)
                            {
                                isAtrracting = true;
                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(target.transform.position.x, transform.position.y), attractionForce * Time.deltaTime);
                            }
                            else
                            {
                                isAtrracting = true;
                                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, target.transform.position.y), attractionForce * Time.deltaTime);
                                if (collision.onMagnet)
                                {
                                    Debug.Log("123");
                                    GetComponent<Rigidbody2D>().gravityScale = 0;
                                }
                                else
                                {
                                    GetComponent<Rigidbody2D>().gravityScale = originGravity;
                                }
                            }

                        }
                        else
                        {
                            // 如果检测到磁铁在玩家的检测范围内，返回
                            if (collision.onMagnet)
                            {
                                return;  // 如果目标物体是检测到的磁铁，则不再施加力
                            }
                            // 吸引物体到玩家位置
                            Vector2 directionToPlayer = (transform.position - target.transform.position).normalized;
                            rb.AddForce(directionToPlayer * attractionForce);
                        }
                    }
                }
            }
        }
    }

    private void Repulse()
    {
        RepulseEffect();
        // 如果没有目标物体，直接返回
        if (collision.targetObject == null)
            return;

        Rigidbody2D targetRb = collision.targetObject.GetComponent<Rigidbody2D>();
        if (collision.targetObject.GetComponent<MagnetInEnviroment>().pole == currentPole)
        {
            if (targetRb == null)
                return;

            // 计算从玩家到目标物体的方向
            Vector2 repulseDirection = (targetRb.transform.position - transform.position).normalized;



            // 如果目标物体设置为kinematic
            if (targetRb.isKinematic)
            {
                if (collision.isInX)
                {
                    if (targetRb.transform.position.x > transform.position.x)
                    {
                        // 如果目标物体在玩家的右边
                        repulseDirection = Vector2.left;  // 对玩家施加向左的力
                    }
                    else
                    {
                        // 如果目标物体在玩家的左边
                        repulseDirection = Vector2.right; // 对玩家施加向右的力
                    }
                    // 对玩家施加反方向的力，实现反弹效果
                    GetComponent<Rigidbody2D>().AddForce(repulseDirection * repulseForce, ForceMode2D.Impulse);
                    GetComponent<Rigidbody2D>().AddForce(Vector2.up * upForceInRepulse, ForceMode2D.Impulse);
                    FindObjectOfType<GhostTrail>().ShowGhost();
                    //GetComponent<Rigidbody2D>().velocity += repulseDirection * repulseForce; 
                }
                else 
                {
                    if (targetRb.transform.position.y > transform.position.y)
                    {
                        // 如果目标物体在玩家的上边
                        repulseDirection = Vector2.down;  // 对玩家施加向左的力
                    }
                    else
                    {
                        // 如果目标物体在玩家的左边
                        repulseDirection = Vector2.up; // 对玩家施加向右的力
                    }
                    // 对玩家施加反方向的力，实现反弹效果
                    GetComponent<Rigidbody2D>().AddForce(repulseDirection * repulseForce, ForceMode2D.Impulse);
                    FindObjectOfType<GhostTrail>().ShowGhost();
                    //GetComponent<Rigidbody2D>().AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                }
            }
            else
            {
                // 否则，对目标物体施加力，使其远离玩家
                targetRb.AddForce(repulseDirection * repulseForce, ForceMode2D.Impulse);
            } 
        }
    }

    public void RepulseEffect()
    {
        Camera.main.transform.DOComplete();
        Camera.main.transform.DOShakePosition(.2f, .5f, 14, 90, false, true);
        FindObjectOfType<RippleEffect>().Emit(Camera.main.WorldToViewportPoint(transform.position));
        StartCoroutine(DashWait());
    }
    IEnumerator DashWait()
    {
        //FindObjectOfType<GhostTrail>().ShowGhost();
        StartCoroutine(GroundDash());
        //DOVirtual.Float(14, 0, .8f, RigidbodyDrag);

        //dashParticle.Play();

        yield return new WaitForSeconds(.3f);

        //dashParticle.Stop();
        
       
       
    }

    IEnumerator GroundDash()
    {
        yield return new WaitForSeconds(.15f);
       
    }

    void RigidbodyDrag(float x)
    {
        rb.drag = x;
    }
}
