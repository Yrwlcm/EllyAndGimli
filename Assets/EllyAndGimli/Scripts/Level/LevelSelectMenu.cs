using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;

public class LevelSelectMenu : MonoBehaviour
{
    public GameObject levelButtonPrefab;
    public Transform gridParent;
    public List<LevelData> levels;

    void Start()
    {
        foreach (var level in levels)
        {
            GameObject go = Instantiate(levelButtonPrefab, gridParent);
            var button = go.GetComponent<Button>();

            // Обновлённые пути из-за вложенности
            var content = go.transform.Find("Content");
            var image = content.Find("Preview").GetComponent<Image>();
            var label = content.Find("Label").GetComponent<TMP_Text>();

            image.sprite = level.preview;
            label.text = level.displayName;

            string sceneName = level.SceneName;
            button.onClick.AddListener(() => SceneManager.LoadScene(sceneName));
        }
    }
}