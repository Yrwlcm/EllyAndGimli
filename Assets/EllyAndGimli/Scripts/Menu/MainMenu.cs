using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nextLevelSceneName;
    public string levelSelectSceneName;
    
    public void StartGame()
    {
        SceneManager.LoadScene(nextLevelSceneName);
    }

    public void OpenLevelSelect()
    {
        SceneManager.LoadScene(levelSelectSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}