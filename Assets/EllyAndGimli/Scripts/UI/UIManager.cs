using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject victoryScreen;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ShowVictory()
    {
        if (victoryScreen != null)
            victoryScreen.SetActive(true);
    }
}