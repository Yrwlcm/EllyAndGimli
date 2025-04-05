using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Serialization;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    
    private bool ellyIn;
    private bool gimliIn;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Elly")) 
            ellyIn = true;
        if (other.CompareTag("Gimli")) 
            gimliIn = true;

        CheckIfBothReady();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Elly")) 
            ellyIn = false;
        if (other.CompareTag("Gimli")) 
            gimliIn = false;
    }

    private void CheckIfBothReady()
    {
        if (!ellyIn || !gimliIn) 
            return;
        
        if (!string.IsNullOrEmpty(nextLevelName))
            SceneManager.LoadScene(nextLevelName);
        else
            StartCoroutine(ShowVictory());
    }

    private static IEnumerator ShowVictory()
    {
        UIManager.Instance.ShowVictory();
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("MainMenu");
    }
}