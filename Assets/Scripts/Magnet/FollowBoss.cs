using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowBoss : MonoBehaviour
{
    public GameObject boss;
    [SerializeField]
    private bool isTouchingCeiling = false;
    public GameObject ceiling;
    private float initialDistanceToCeiling;
    // Start is called before the first frame update
    void Start()
    {
        initialDistanceToCeiling = transform.position.y - ceiling.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.x = boss.transform.position.x;
        newPosition.y = ceiling.transform.position.y + initialDistanceToCeiling; // 加上初始距离，而不是减去
        transform.position = newPosition;

    }

    

}
