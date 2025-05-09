using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Serialization;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    
    private bool ellyIn;
    private bool gimliIn;
    private bool crystalsCollected;
    
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
        if (!ellyIn || !gimliIn || !crystalsCollected) 
            return;
        
        if (!string.IsNullOrEmpty(nextLevelName))
            LevelManager.Instance.LoadNextScene(nextLevelName);
        else
            LevelManager.Instance.LoadNextScene("MainMenu");
    }

    // private static IEnumerator ShowVictory()
    // {
    //     UIManager.Instance.ShowVictory();
    //     yield return new WaitForSeconds(5f);
    //     SceneManager.LoadScene("MainMenu");
    // }

    public void OnAllCrystalsCollected()
    {
        crystalsCollected = true;
    }
}