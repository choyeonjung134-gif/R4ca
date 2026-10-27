using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject gameOverPanel;
    public string titleSceneName = "Title";

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;      // �ٽ� �ҷ��͵� �ð� ����
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void GoTitle()
    {
        SceneManager.LoadScene(titleSceneName);
        Time.timeScale = 1f;
    }
}

// Start is called once before the first execution of Update after the MonoBehaviour is created
