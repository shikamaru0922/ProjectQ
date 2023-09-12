using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject deathMenu;
    public GameObject menu;
    public GameObject finishMenu;

    private void Update()
    {
        if (GameManager.Instance.PlayerIsDead && deathMenu.active == false)
            deathMenu.active = true;

        if (GameManager.Instance.bossIsDead == true) 
        {
            ShowFinishMenu();
        }

    }

    public void AgainButton() 
    {
    
    }


    public void ReloadCurrentScene()
    {

        // 重置GameManager的状态
        GameManager.Instance.Reset();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
       
    }

    public void ShowNormalMenu() 
    {
        if (menu.active == false)
            menu.active = true;
    }

    public void ShowFinishMenu()
    {
        if (finishMenu.active == false)
            finishMenu.active = true;
    }

    // 退出游戏的方法
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // 如果在Unity编辑器中，停止播放
        #else
            Application.Quit();  // 如果是构建版本，退出游戏
        #endif
    }

    

    public void CloseCurrentUI()
    {
        // 检查menuUI是否已设置
        if (menu)
        {
            menu.SetActive(false);
        }
        else
        {
            Debug.LogWarning("menuUI is not assigned!");
        }
    }
}
