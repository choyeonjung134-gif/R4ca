using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenu : MonoBehaviour
{
    public string gameSceneName = "Round1";
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();   // 빌드에서만 동작
    }
}
