using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class LevelSelectMenu : MonoBehaviour
{
    public void LoadFirstLevel()
    {
        LevelManager.Instance.LoadNextScene("Level1");
    }
    
    public void LoadSecondLevel()
    {
        LevelManager.Instance.LoadNextScene("Level2");
    }
    
    public void LoadThirdLevel()
    {
        LevelManager.Instance.LoadNextScene("Level3");
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}