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

    //public Color originalColor = Color.white;   // 设置玩家的原始颜色
    public Color n_PoleTargetColor = Color.red;       // 设置目标颜色，可以根据你的选择进行更改
    public Color s_PoleTargetColor = Color.blue;
    public Color n_PoleOriginalColor;  // 设置为N_Pole的原始颜色
    public Color s_PoleOriginalColor = Color.white;  // 设置为S_Pole的原始颜色
    public float colorChangeDuration = 1.0f;    // 颜色变化的持续时间

    public SpriteRenderer playerSpriteRenderer;

    private MusicInPlayer musicInPlayer;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collision = GetComponent<Collision>();
        originGravity = GetComponent<Rigidbody2D>().gravityScale;
        musicInPlayer = GetComponent<MusicInPlayer>();
//playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
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

        if (isAtrracting) // 替换为你的条件
        {
            if (!musicInPlayer.isAttractSoundPlaying)
            {
                musicInPlayer.PlayAttractSound();
                Debug.Log(111);
                musicInPlayer.isAttractSoundPlaying = true;
            }
        }
        else if (isAtrracting == false) // 替换为你的条件
        {
            if (musicInPlayer.isAttractSoundPlaying)
            {
                musicInPlayer.StopAttractSound();
                musicInPlayer.isAttractSoundPlaying = false;
            }
        }

    }

    private void TogglePole()
    {
        currentPole = (currentPole == PoleType.N_Pole) ? PoleType.S_Pole : PoleType.N_Pole;
        GetComponent<Rigidbody2D>().gravityScale = originGravity;

        // 当磁极改变时，立即设置为目标颜色
        Color targetColor = (currentPole == PoleType.N_Pole) ? n_PoleTargetColor : s_PoleTargetColor;
        playerSpriteRenderer.color = targetColor;

        // 然后开始颜色过渡，使其逐渐回到对应的原始颜色
        Color originalColor = (currentPole == PoleType.N_Pole) ? n_PoleOriginalColor : s_PoleOriginalColor;
        StartCoroutine(ChangeColorBackToOriginal(targetColor, originalColor));
    }

    // 请注意，我们现在需要传递两个颜色参数给这个协程
    IEnumerator ChangeColorBackToOriginal(Color startColor, Color endColor)
    {
        float elapsed = 0f;  // 计时器，追踪过渡的进度

        while (elapsed < colorChangeDuration)
        {
            elapsed += Time.deltaTime;
            float percentage = Mathf.Clamp01(elapsed / colorChangeDuration);  // 计算过渡进度的百分比

            // 使用Lerp进行颜色插值
            playerSpriteRenderer.color = Color.Lerp(startColor, endColor, percentage);

            yield return null;  // 等待下一帧
        }

        playerSpriteRenderer.color = endColor;  // 确保在动画结束时精确设置为目标颜色
    }
    private void SetMagneticMode(MagneticMode mode)
    {
        currentMode = mode;
    }

    private void Attract()
    {
        isAtrracting = true;
        if (collision.targetObject != null)
        {
            GameObject target = collision.targetObject;
            MagnetInEnviroment magneticObject = target.GetComponent<MagnetInEnviroment>();

            if (magneticObject != null)
            {
                // 检查物体的极性是否与玩家不同
                if (magneticObject.pole != currentPole)
                {
                    Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        // 检查目标物体是否有一个称为EndPoint的子物体
                        Transform endPoint = target.transform.Find("EndPoint");
                        Vector2 targetPosition;

                        if (endPoint != null) // 如果有EndPoint子物体
                        {
                            targetPosition = endPoint.position;
                            isAtrracting = true;
                            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                            GetComponent<Rigidbody2D>().gravityScale = 0;
                            transform.position = Vector2.MoveTowards(transform.position, targetPosition, attractionForce * Time.deltaTime);
                        }
                        else
                        {
                            targetPosition = target.transform.position;
                            if (rb.isKinematic) // 如果物体是kinematic
                            {
                                isAtrracting = true;
                                if (collision.isInX)
                                {
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(targetPosition.x, transform.position.y), attractionForce * Time.deltaTime);
                                }
                                else
                                {
                                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, targetPosition.y), attractionForce * Time.deltaTime);
                                    if (collision.onMagnet)
                                    {
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
                                if (collision.onMagnet)
                                {
                                    return;  // 如果目标物体是检测到的磁铁，则不再施加力
                                }
                                isAtrracting = true;
                                Vector2 directionToPlayer = (transform.position - target.transform.position).normalized;
                                rb.AddForce(directionToPlayer * attractionForce);
                            }
                        }
                    }
                }
            }
        }
    }

    private void Repulse()
    {
        RepulseEffect();
        musicInPlayer.PlayRepelSound();
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
