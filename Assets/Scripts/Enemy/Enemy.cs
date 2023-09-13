using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    protected Rigidbody2D rb;
    protected Animator anim;


    public float detectionRadius = 5f; // ̽����ҵİ뾶
    public LayerMask playerLayer;      // ������ڵĲ�
    public float offsetY;
    [SerializeField]
    private Vector3 originScale;

    public bool findPlayer;


    public float maxHealth;
    public float currentHealth;
    private SpriteRenderer spriteRenderer;

    private PlayerState playerState; // ��ҵ�״̬������
    public float damagePerSecond;

    public LayerMask magnetLayer;
    public bool touchRightWall;
    public bool touchLeftWall;
    public Vector2 rightOffset, leftOffset;
    public float collisionRadius;
    public LayerMask groundLayer;
    // Start is called before the first frame update

    private float cooldownTimer = 0f;  // ��ʱ��
    public float cooldownDuration = 0.5f;  // ��ת��������ȴʱ�䣨���磬0.5�룩
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = chaseSpeed;
        originScale = transform.localScale;
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        currentHealth = maxHealth;
        faceDir = new Vector3(-transform.localScale.x / transform.localScale.x, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //faceDir = new Vector3(-transform.localScale.x/transform.localScale.x, 0, 0);
        
        DetectPlayer();
        

        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

    
    }
    private void FixedUpdate()
    {
        Move();
    }
    public virtual void Move() 
    {
        if (touchRightWall && !touchLeftWall)
        {
            faceDir.x = -1;
        }
        else if (touchLeftWall && !touchRightWall)
        {
            faceDir.x = 1;
        }
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((magnetLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
            TakeDamage(1);

        if ((groundLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
         
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((groundLayer & 1 << collision.gameObject.layer) == 1 << collision.gameObject.layer)
        {
            
            
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerState = collision.gameObject.GetComponent<PlayerState>();
            if (playerState != null)
            {
                playerState.currentHealth -= damagePerSecond * Time.deltaTime;
            }
        }
    }
    public virtual void DetectPlayer()
    {
        // �������Ƿ���̽�ⷶΧ��
        Collider2D playerDetected = Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y+offsetY), detectionRadius, playerLayer);

        if (playerDetected)
        {
            findPlayer = true;
            // ���㳯����ҵķ���
            float directionToPlayer = Mathf.Sign(playerDetected.transform.position.x - transform.position.x);

            if (playerDetected.transform.position.x - transform.position.x > 0.1f || playerDetected.transform.position.x - transform.position.x < -0.1f)
            {
                // ���³���
                faceDir = new Vector3(directionToPlayer, 0, 0);
                // �����Ҫ��Ҳ���Ը���localScale�����µ��˵�ͼ����
                transform.localScale = new Vector3(directionToPlayer * originScale.x, originScale.y, originScale.z);
            }


        }
        else
            findPlayer = false;
    }

    public virtual void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();

        }
        else
        {
            anim.SetTrigger("GetHit");
            
        }
    }

    public IEnumerator DamageFlash()
    {
        for (int i = 0; i < 3; i++)  // ��˸3��
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);  // ����Ϊ��͸���ĺ�ɫ
            yield return new WaitForSeconds(0.1f);  // �ȴ�0.1��
            spriteRenderer.color = Color.white;  // ����Ϊ������ɫ
            yield return new WaitForSeconds(0.1f);  // �ȴ�0.1��
        }
    }
    public void Die()
    {
        anim.SetTrigger("isDead");
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(new Vector3 (transform.position.x, transform.position.y + offsetY,0),detectionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }

}
