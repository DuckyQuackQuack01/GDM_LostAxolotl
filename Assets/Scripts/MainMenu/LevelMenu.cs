using UnityEngine;

public class MainMenuLevelSelector : MonoBehaviour
{
    public MovePlayerIcon playerMover;

    [System.Serializable]
    public class LevelData
    {
        public string sceneName;
        public Transform destinationPoint;
    }

    public LevelData[] levels;

    public void SelectLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length) return;

        playerMover.MoveToPointAndLoad(
            levels[levelIndex].destinationPoint.position,
            levels[levelIndex].sceneName
        );
    }
}