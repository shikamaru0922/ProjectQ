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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  // �����Unity�༭���У�ֹͣ����
#else
            Application.Quit();  // ����ǹ����汾���˳���Ϸ
#endif
    }
}
