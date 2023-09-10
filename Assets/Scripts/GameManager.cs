using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool bossIsHit;
    public bool bossIsDead;

    public bool PlayerIsDead;

    public GameObject menuUI;  // ָ��˵�UI������
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

    private void Update()
    {
        if (PlayerIsDead)
        {
            
            StartCoroutine(PauseGameGradually());
            PlayerIsDead = false;  // Ϊ�˷�ֹ�ظ�����s
        }
    }

    IEnumerator PauseGameGradually()
    {
        Debug.Log("��ʼ��ͣ��Ϸ");
        float startTime = Time.unscaledTime;  // ʹ��unscaledTime����Ϊ������timeScale��Ӱ��
        float initialTimeScale = Time.timeScale;

        while (Time.unscaledTime - startTime < slowDownDuration)
        {
            float elapsed = Time.unscaledTime - startTime;
            Time.timeScale = Mathf.Lerp(initialTimeScale, 0, elapsed / slowDownDuration);
            yield return null;
        }

        Time.timeScale = 0;
        //ShowMenu();
    }

    void ShowMenu()
    {
        // ��ʾ�˵�UI
        menuUI.SetActive(true);
    }
}
