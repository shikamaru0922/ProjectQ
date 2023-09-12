using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem :Enemy
{
    //public GameObject laser;

    public Transform[] targetPositions; // 在Unity编辑器中设置这些位置
    [SerializeField]
    private Transform currentTarget;
    [SerializeField]
    private bool isMoving = false; // 添加这个新的成员变量

    public bool isWaiting = false;
    private bool isWaitingForNextTarget = false; // 新的成员变量来防止协程重复调用



    public GameObject laserPrefab; // 这是你的激光预制体，你可以在Unity编辑器中为这个变量分配预制体
    public Transform laserPoint; // 这是Golem中的激光生成点
    private GameObject currentLaser; // 这是当前生成的激光实例

    [SerializeField]
    private float laserTrackingSpeed = 0.1f;  // 控制激光追踪的速度，可以在Inspector中调整

    public float laserCooldownDuration = 3.0f;
    private float lastLaserFireTime = -5.0f; // 初始化为一个较小的值

    private Vector2 laserTargetDirection;

    public float waitingTime;

     //new private LayerMask playerLayer; // 设置一个LayerMask，使其只与玩家层碰撞。
    [SerializeField] private int damageToTake = 10; // 这是玩家对敌人造成的伤害。
    [SerializeField] private float bounceForce = 10f; // 这是玩家受到撞击时的向上弹力。


    private void Start()
    {
        currentHealth = maxHealth;
        PickRandomTarget();
    }
    private void Update()
    {
        DetectPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((playerLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)  // 检查是否与玩家层碰撞。
        {
            TakeDamage(damageToTake);  // 敌人扣血。
            GameManager.Instance.bossIsHit = true;
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce); // 给玩家一个向上的弹力。
            }
        }
    }


    public override void Move()
    {
        if (Vector2.Distance(transform.position, currentTarget.position) < 0.5f)
        {
            StartCoroutine(WaitAndPickNewTarget());
        }
        if (isMoving == false)
        {
            rb.velocity = Vector2.zero;
        }
        else
        {
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
        }
    }

    public override void DetectPlayer()
    { 
        Transform playerTransform = CheckForPlayer();
        FireLaser(playerTransform);
    }

    private Transform CheckForPlayer()
    {
        // 检查玩家是否在探测范围内
        Collider2D playerDetected = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + offsetY), detectionRadius, playerLayer);

        if (playerDetected)
        {
            findPlayer = true;
            anim.SetBool("FindPlayer", true);
            return playerDetected.transform;

        }
        else
        {
            findPlayer = false;
            return null;
        }
    }

    private void FireLaser(Transform target)
    {
        if (target != null)
        {
            // 获取指向玩家的方向
            Vector2 directionToPlayer = (target.position - laserPoint.position).normalized;

            // 使用Lerp插值来平滑地改变激光的方向
            laserTargetDirection = Vector2.Lerp(laserTargetDirection, directionToPlayer, laserTrackingSpeed);

            // 计算旋转角度
            float angle = Mathf.Atan2(laserTargetDirection.y, laserTargetDirection.x) * Mathf.Rad2Deg;

            // 如果从上次发射激光到现在的时间大于等于冷却时间
            if (Time.time - lastLaserFireTime >= laserCooldownDuration)
            {

                // 如果当前还没有激光实例，则创建一个
                if (currentLaser == null)
                {
                    currentLaser = Instantiate(laserPrefab, laserPoint.position, Quaternion.Euler(new Vector3(0, 0, angle)));
                }
                currentLaser.SetActive(true); // 确保激光是活动的
                lastLaserFireTime = Time.time;  // 更新上次发射激光的时间
            }

            // 如果有激光实例，则更新它的位置和方向
            if (currentLaser != null)
            {
                currentLaser.transform.position = laserPoint.position;
                currentLaser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                //currentLaser.SetActive(true); // 确保激光是活动的
            }
        }
        else
        {
            // 玩家不在探测范围内时，禁用laser
            if (currentLaser != null)
            {
                currentLaser.SetActive(false);
            }
        }

    }
  private void PickRandomTarget()
    {
        if (isMoving) return; // 如果正在移动，直接返回，不做任何操作

        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, targetPositions.Length);
        } while (targetPositions[randomIndex] == currentTarget);

        currentTarget = targetPositions[randomIndex];

        // 根据目标位置改变移动方向
        faceDir = new Vector3(Mathf.Sign(currentTarget.position.x - transform.position.x), 0, 0);

        // 更新Golem的朝向
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * faceDir.x;
        transform.localScale = newScale;

        isMoving = true; // 设置为正在移动状态
        isWaiting = false;
    }

    public override void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
            anim.SetTrigger("GetHit");
            GameManager.Instance.bossIsDead = true;
        }
        else
        {
            StartCoroutine(DamageFlash());
        }
    }
    private IEnumerator WaitAndPickNewTarget()
    {
        if (isWaitingForNextTarget) yield break; // 如果已经有一个等待协程在运行，直接退出

        isWaitingForNextTarget = true; // 标记为等待中
        isMoving = false; // 到达目标后，设置为非移动状态
        isWaiting = true;
        yield return new WaitForSeconds(waitingTime);
        PickRandomTarget();
        isWaitingForNextTarget = false; // 重置标记

    }




}
