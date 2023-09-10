using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool bossIsHit;
    public bool bossIsDead;

    public bool PlayerIsDead;

    public GameObject menuUI;  // 指向菜单UI的引用
    public float slowDownDuration = 1.0f;  // 游戏暂停的过渡时间s

    private void Awake()
    {
        // 如果单例还没有被设置，设置它
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 确保单例不会被销毁
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
            PlayerIsDead = false;  // 为了防止重复触发s
        }
    }

    IEnumerator PauseGameGradually()
    {
        Debug.Log("开始暂停游戏");
        float startTime = Time.unscaledTime;  // 使用unscaledTime，因为它不受timeScale的影响
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
        // 显示菜单UI
        menuUI.SetActive(true);
    }
}
