
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public int requiredKills = 3;
    public string nextStageName = "Round2";

    private bool stageChanging = false;

    void Update()
    {
        if (stageChanging)
            return;

        if (Health.killCount >= requiredKills)
        {
            stageChanging = true;

            SceneManager.LoadScene(nextStageName);
        }
    }
}
