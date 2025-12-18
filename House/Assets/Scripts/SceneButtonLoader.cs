using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButtonLoader : MonoBehaviour
{
    [Header("이동할 씬 이름")]
    public string sceneName;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }

    // 선택: 게임 종료 버튼
    public void QuitGame()
    {
        Application.Quit();
    }
}