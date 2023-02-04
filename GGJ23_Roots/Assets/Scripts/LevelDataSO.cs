using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level Data")]
public class LevelDataSO : ScriptableObject
{
    [SerializeField] private GameStates _startGameState;

    public GameStates StartGameState => _startGameState;
    public int CurrentLevelIndex {get; set;}
}