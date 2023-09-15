using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public string sceneNameToLoad = "Level";  // ������д�����Ϸ����������

    public void StartGame()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }

    public void QuitGame()
    {
            Application.Quit();  // ����ǹ����汾���˳���Ϸ

    }
}
