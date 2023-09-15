using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameButton : MonoBehaviour
{
    public string sceneNameToLoad = "Level";  // 这里填写你的游戏场景的名称

    public void StartGame()
    {
        SceneManager.LoadScene(sceneNameToLoad);
    }

    public void QuitGame()
    {
            Application.Quit();  // 如果是构建版本，退出游戏

    }
}
