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

        // ����GameManager��״̬
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

    // �˳���Ϸ�ķ���
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;  // �����Unity�༭���У�ֹͣ����
        #else
            Application.Quit();  // ����ǹ����汾���˳���Ϸ
        #endif
    }

    

    public void CloseCurrentUI()
    {
        // ���menuUI�Ƿ�������
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
