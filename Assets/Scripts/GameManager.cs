using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool bossIsHit;
    public bool bossIsDead;

    public bool PlayerIsDead;

    //public GameObject menuUI;  // ָ��˵�UI������
    public float slowDownDuration = 1.0f;  // ��Ϸ��ͣ�Ĺ���ʱ��s

    private void Awake()
    {
        // ���������û�б����ã�������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ȷ���������ᱻ����
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
       

    }

    private void Start()
    {
    }

    private void Update()
    {
        if (PlayerIsDead)
        {
            //StartCoroutine(PauseGameGradually());
            //PlayerIsDead = false;  // Ϊ�˷�ֹ�ظ�����s
            //ShowMenu();
            Time.timeScale = 0.001f;
        }
    }

    IEnumerator PauseGameGradually()
    {
        float startTime = Time.unscaledTime;  // ʹ��unscaledTime����Ϊ������timeScale��Ӱ��
        float initialTimeScale = Time.timeScale;

        while (Time.unscaledTime - startTime < slowDownDuration)
        {
            float elapsed = Time.unscaledTime - startTime;
            
            yield return null;
        }
        //ShowMenu();
    }

    public void Reset()
    {
        bossIsHit = false;
        bossIsDead = false;
        PlayerIsDead = false;
        Time.timeScale = 1;  // ����Ϸʱ��ָ��������ٶ�
        Debug.Log(Time.timeScale);
    }

    void ShowMenu()
    {
        // ��ʾ�˵�UI
        //menuUI.SetActive(true);
    }
}
