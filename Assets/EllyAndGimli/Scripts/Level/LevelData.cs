using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public Sprite preview;
    public string displayName;
    public string SceneName => sceneName;
    
    [SerializeField] private string sceneName;
}