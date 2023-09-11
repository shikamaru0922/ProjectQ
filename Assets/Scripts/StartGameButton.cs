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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // 如果在Unity编辑器中，停止播放
#else
            Application.Quit();  // 如果是构建版本，退出游戏
#endif
    }
}
