using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float chaseSpeed;
    public float currentSpeed;
    public Vector3 faceDir;
    private Rigidbody2D rb;
    protected Animator anim;


    public float detectionRadius = 5f; // ̽����ҵİ뾶
    public LayerMask playerLayer;      // ������ڵĲ�
    public float offsetY;
    [SerializeField]
    private Vector3 originScale;

    public bool findPlayer;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = chaseSpeed;
        originScale = transform.localScale;
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(new Vector3 (transform.position.x, transform.position.y + offsetY,0),detectionRadius);
    }

}
