using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class LevelSelectMenu : MonoBehaviour
{
    public void LoadLevel(int levelNumber)
    {
        LevelManager.Instance.LoadNextScene("Level" + levelNumber);
    }
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}