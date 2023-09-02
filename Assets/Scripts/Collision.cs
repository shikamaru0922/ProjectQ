using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{

    [Header("Layers")]
    public LayerMask groundLayer;
    public LayerMask magnetLayer;

    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public bool onMagnet;
    public int wallSide;

    [Space]

    [Header("Collision")]

    public float collisionRadius = 0.25f;
    public Vector2 bottomOffset, rightOffset, leftOffset,upOffset;
    private Color debugCollisionColor = Color.red;


    [Space]

    [Header("Magnet Collision")]
    public Collider2D rightC;
    public Collider2D leftC;
    public Collider2D upC;
    public Collider2D downC;

    [SerializeField]
    private GameObject magnetInRight;
    [SerializeField]
    private GameObject magnetInLeft;
    [SerializeField]
    private GameObject magnetInUp;
    [SerializeField]
    private GameObject magnetInDown;

    public GameObject targetObject;
    public bool isInX; //1 in x, 0 in Y

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {  
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, magnetLayer);
        onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) 
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);

        wallSide = onRightWall ? -1 : 1;

        onMagnet = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, magnetLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, magnetLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, magnetLayer)
            || Physics2D.OverlapCircle((Vector2)transform.position + upOffset, collisionRadius, magnetLayer);

        CheckTargetObject();

    }

    void CheckTargetObject() 
    {
        /* GameObject player = this.gameObject;  // 假设此脚本附加在玩家上

         magnetInRight = rightC.gameObject.GetComponent<MagnetTrigger>().targetObject;
         magnetInLeft = leftC.gameObject.GetComponent<MagnetTrigger>().targetObject;
         magnetInUp = upC.gameObject.GetComponent<MagnetTrigger>().targetObject;
         magnetInDown = downC.gameObject.GetComponent<MagnetTrigger>().targetObject;

         float minDistance = float.MaxValue; // 设置一个初始大的距离值

         // 用于比较距离的数组
         GameObject[] magnets = new GameObject[] { magnetInRight, magnetInLeft, magnetInUp, magnetInDown };

         foreach (var magnet in magnets)
         {
             if (magnet != null) // 确保磁铁不为空
             {
                 float distance = Vector2.Distance(player.transform.position, magnet.transform.position);
                 if (distance < minDistance)
                 {
                     minDistance = distance;
                     targetObject = magnet;

                 }
             }
         }
         if (magnetInRight == null && magnetInLeft == null && magnetInUp == null && magnetInDown == null)
         {
             targetObject = null;
         }*/

        GameObject player = this.gameObject;  // 假设此脚本附加在玩家上

        MagnetTrigger magnetRightTrigger = rightC.gameObject.GetComponent<MagnetTrigger>();
        MagnetTrigger magnetLeftTrigger = leftC.gameObject.GetComponent<MagnetTrigger>();
        MagnetTrigger magnetUpTrigger = upC.gameObject.GetComponent<MagnetTrigger>();
        MagnetTrigger magnetDownTrigger = downC.gameObject.GetComponent<MagnetTrigger>();

        magnetInRight = magnetRightTrigger.targetObject;
        magnetInLeft = magnetLeftTrigger.targetObject;
        magnetInUp = magnetUpTrigger.targetObject;
        magnetInDown = magnetDownTrigger.targetObject;

        float minDistance = float.MaxValue; // 设置一个初始大的距离值
        //Vector2 forceToAdd = Vector2.zero;  // 初始化为零的力

        // 用于比较距离的数组
        GameObject[] magnets = new GameObject[] { magnetInRight, magnetInLeft, magnetInUp, magnetInDown };

        foreach (var magnet in magnets)
        {
            if (magnet != null) // 确保磁铁不为空
            {
                float distance = Vector2.Distance(player.transform.position, magnet.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    targetObject = magnet;

                    // 确定要添加的力的方向
                    if (magnet == magnetInRight || magnet == magnetInLeft)
                    {
                        isInX = true;
                    }
                    else if (magnet == magnetInUp || magnet == magnetInDown)
                    {
                        isInX = false;
                    }
                }
            }
        }
        if (magnetInRight == null && magnetInLeft == null && magnetInUp == null && magnetInDown == null)
        {
            targetObject = null;
        }

        // 这里，你可以使用forceToAdd为玩家添加力，例如：
        //player.GetComponent<Rigidbody2D>().AddForce(forceToAdd);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position  + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + upOffset, collisionRadius);
    }
}
