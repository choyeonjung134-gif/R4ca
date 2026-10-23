
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class TitleMenu : MonoBehaviour
{
    public string gameSceneName = "Round1";
    public string titleSceneName = "Title";

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (SceneManager.GetActiveScene().name == titleSceneName)
            {
                QuitGame();
            }
            else
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene(titleSceneName);
            }
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("게임 종료");
    }
}
