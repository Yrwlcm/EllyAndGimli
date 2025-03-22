using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    public string nextLevelName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(nextLevelName))
            {
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                StartCoroutine(ShowVictoryAndReturn());
            }
        }
    }

    private IEnumerator ShowVictoryAndReturn()
    {
        UIManager.Instance.ShowVictory();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }
}