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


    public float detectionRadius = 5f; // 探测玩家的半径
    public LayerMask playerLayer;      // 玩家所在的层
    public float offsetY;
    [SerializeField]
    private Vector3 originScale;

    public bool findPlayer;


    public float maxHealth;
    public float currentHealth;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
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
        
    }

    // Update is called once per frame
    void Update()
    {
        faceDir = new Vector3(-transform.localScale.x/transform.localScale.x, 0, 0);
        
        DetectPlayer();
    }
    private void FixedUpdate()
    {
        Move();
    }
    public virtual void Move() 
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }


    public virtual void DetectPlayer()
    {
        // 检查玩家是否在探测范围内
        Collider2D playerDetected = Physics2D.OverlapCircle(new Vector2(transform.position.x,transform.position.y+offsetY), detectionRadius, playerLayer);

        if (playerDetected)
        {
            findPlayer = true;
            // 计算朝向玩家的方向
            float directionToPlayer = Mathf.Sign(playerDetected.transform.position.x - transform.position.x);

            if (playerDetected.transform.position.x - transform.position.x > 0.1f || playerDetected.transform.position.x - transform.position.x < -0.1f)
            {
                // 更新朝向
                faceDir = new Vector3(directionToPlayer, 0, 0);
                // 如果需要，也可以更改localScale来更新敌人的图像朝向
                transform.localScale = new Vector3(directionToPlayer * originScale.x, originScale.y, originScale.z);
            }


        }
        else
            findPlayer = false;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageFlash());
        }
    }

    private IEnumerator DamageFlash()
    {
        for (int i = 0; i < 3; i++)  // 闪烁3次
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);  // 设置为半透明的红色
            yield return new WaitForSeconds(0.1f);  // 等待0.1秒
            spriteRenderer.color = Color.white;  // 设置为正常颜色
            yield return new WaitForSeconds(0.1f);  // 等待0.1秒
        }
    }
    private void Die()
    {
        // 你可以在这里处理敌人的死亡，例如播放动画，播放声音等。
        Destroy(gameObject);  // 这会销毁当前的敌人对象，你可以根据需要进行修改。
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(new Vector3 (transform.position.x, transform.position.y + offsetY,0),detectionRadius);
    }

}
