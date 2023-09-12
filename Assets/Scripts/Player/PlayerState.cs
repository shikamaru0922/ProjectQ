using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static MagneticPlayer;
using Sharklib.ProgressBar;

public class PlayerState : MonoBehaviour
{
    public float maxHealth=100;
    public float currentHealth;
    public MagneticPlayer magneticPlayer;
    public ProgressBarPro progressBar;
    public BarViewColor viewColor;
    public Color pole_N;
    public Color pole_S;

    private MusicInPlayer musicInplayer;
    private PlayerMovement playerMovement;
    private SpriteRenderer spriteRenderer;

    [SerializeField]private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        // ��ȡ���������
        playerMovement = GetComponent<PlayerMovement>();
        musicInplayer = GetComponent<MusicInPlayer>();
        magneticPlayer = GetComponent<MagneticPlayer>();
        spriteRenderer = magneticPlayer.playerSpriteRenderer;
        animator = magneticPlayer.playerSpriteRenderer.GetComponent<Animator>();

        if (playerMovement == null || magneticPlayer == null || spriteRenderer == null)
        {
            Debug.LogError("Required components are missing on the player!");
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) 
        {
            currentHealth = 0;
            Dead();
            
        }

        if (progressBar!= null)
        progressBar.SetValue(currentHealth / maxHealth);

        if (magneticPlayer.currentPole == PoleType.N_Pole) 
        {
            viewColor.defaultColor = pole_N;
            viewColor.UpdateColor();

        }
        if (magneticPlayer.currentPole == PoleType.S_Pole)
        {
            viewColor.defaultColor = pole_S;
            viewColor.UpdateColor();

        }
    }

    void Dead() 
    {
        // ����PlayerMovement��MagneticPlayerΪ�Ǽ���״̬
        playerMovement.enabled = false;
        magneticPlayer.enabled = false;

        // �������ɫ����Ϊ��ɫ
        spriteRenderer.color = Color.gray;

        musicInplayer.PlayDeathSound();

        // ��ʼ���ٶ����ͱ�ɫ��Э��
        StartCoroutine(SlowDownAnimationAndFadeToGray());
        GameManager.Instance.PlayerIsDead = true;
    }
    IEnumerator SlowDownAnimationAndFadeToGray()
    {
        float timeToTransition = 1.0f;  // ���Ե������ֵ���ı���ɵ�ʱ��
        float initialSpeed = animator.speed;
        Color initialColor = spriteRenderer.color;
        Color grayColor = Color.gray;

        float elapsedTime = 0;
        while (elapsedTime < timeToTransition)
        {
            elapsedTime += Time.deltaTime;
            animator.speed = Mathf.Lerp(initialSpeed, 0, elapsedTime / timeToTransition);
            spriteRenderer.color = Color.Lerp(initialColor, grayColor, elapsedTime / timeToTransition);
            yield return null;
        }

        animator.speed = 0;
        animator.enabled = false;
        spriteRenderer.color = grayColor;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ceiling") 
        {
            if (GetComponent<Collision>().onGround) 
            {
                Dead();
            }
        }

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ConveyBelt"))
        {
            Transform endPoint = collision.transform.Find("EndPoint");
            Vector2 targetPosition;
            if (endPoint != null) // �����EndPoint������
            {
                targetPosition = endPoint.position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, collision.gameObject.GetComponent<ConveyBelt>().speed);
            }
        }
    }

}
