using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem :Enemy
{
    //public GameObject laser;

    public Transform[] targetPositions; // ��Unity�༭����������Щλ��
    [SerializeField]
    private Transform currentTarget;
    [SerializeField]
    private bool isMoving = false; // �������µĳ�Ա����

    public bool isWaiting = false;
    private bool isWaitingForNextTarget = false; // �µĳ�Ա��������ֹЭ���ظ�����



    public GameObject laserPrefab; // ������ļ���Ԥ���壬�������Unity�༭����Ϊ�����������Ԥ����
    public Transform laserPoint; // ����Golem�еļ������ɵ�
    private GameObject currentLaser; // ���ǵ�ǰ���ɵļ���ʵ��

    [SerializeField]
    private float laserTrackingSpeed = 0.1f;  // ���Ƽ���׷�ٵ��ٶȣ�������Inspector�е���

    public float laserCooldownDuration = 3.0f;
    private float lastLaserFireTime = -5.0f; // ��ʼ��Ϊһ����С��ֵ

    private Vector2 laserTargetDirection;

    public float waitingTime;

     //new private LayerMask playerLayer; // ����һ��LayerMask��ʹ��ֻ����Ҳ���ײ��
    [SerializeField] private int damageToTake = 10; // ������ҶԵ�����ɵ��˺���
    [SerializeField] private float bounceForce = 10f; // ��������ܵ�ײ��ʱ�����ϵ�����


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
        if ((playerLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)  // ����Ƿ�����Ҳ���ײ��
        {
            TakeDamage(damageToTake);  // ���˿�Ѫ��
            GameManager.Instance.bossIsHit = true;
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce); // �����һ�����ϵĵ�����
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
        // �������Ƿ���̽�ⷶΧ��
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
            // ��ȡָ����ҵķ���
            Vector2 directionToPlayer = (target.position - laserPoint.position).normalized;

            // ʹ��Lerp��ֵ��ƽ���ظı伤��ķ���
            laserTargetDirection = Vector2.Lerp(laserTargetDirection, directionToPlayer, laserTrackingSpeed);

            // ������ת�Ƕ�
            float angle = Mathf.Atan2(laserTargetDirection.y, laserTargetDirection.x) * Mathf.Rad2Deg;

            // ������ϴη��伤�⵽���ڵ�ʱ����ڵ�����ȴʱ��
            if (Time.time - lastLaserFireTime >= laserCooldownDuration)
            {

                // �����ǰ��û�м���ʵ�����򴴽�һ��
                if (currentLaser == null)
                {
                    currentLaser = Instantiate(laserPrefab, laserPoint.position, Quaternion.Euler(new Vector3(0, 0, angle)));
                }
                currentLaser.SetActive(true); // ȷ�������ǻ��
                lastLaserFireTime = Time.time;  // �����ϴη��伤���ʱ��
            }

            // ����м���ʵ�������������λ�úͷ���
            if (currentLaser != null)
            {
                currentLaser.transform.position = laserPoint.position;
                currentLaser.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                //currentLaser.SetActive(true); // ȷ�������ǻ��
            }
        }
        else
        {
            // ��Ҳ���̽�ⷶΧ��ʱ������laser
            if (currentLaser != null)
            {
                currentLaser.SetActive(false);
            }
        }

    }
  private void PickRandomTarget()
    {
        if (isMoving) return; // ��������ƶ���ֱ�ӷ��أ������κβ���

        int randomIndex;
        do
        {
            randomIndex = UnityEngine.Random.Range(0, targetPositions.Length);
        } while (targetPositions[randomIndex] == currentTarget);

        currentTarget = targetPositions[randomIndex];

        // ����Ŀ��λ�øı��ƶ�����
        faceDir = new Vector3(Mathf.Sign(currentTarget.position.x - transform.position.x), 0, 0);

        // ����Golem�ĳ���
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * faceDir.x;
        transform.localScale = newScale;

        isMoving = true; // ����Ϊ�����ƶ�״̬
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
        if (isWaitingForNextTarget) yield break; // ����Ѿ���һ���ȴ�Э�������У�ֱ���˳�

        isWaitingForNextTarget = true; // ���Ϊ�ȴ���
        isMoving = false; // ����Ŀ�������Ϊ���ƶ�״̬
        isWaiting = true;
        yield return new WaitForSeconds(waitingTime);
        PickRandomTarget();
        isWaitingForNextTarget = false; // ���ñ��

    }




}
