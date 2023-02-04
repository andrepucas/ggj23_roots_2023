using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level Data")]
public class LevelDataSO : ScriptableObject
{
    [SerializeField] private GameStates _startGameState;
    [SerializeField] private Level[] _levels;

    public GameStates StartGameState => _startGameState;
    public IReadOnlyCollection<Level> Levels => _levels;
    public int CurrentLevelIndex {get; set;}
}

[System.Serializable]
public struct Level
{
    [SerializeField] private GameObject _levelPrefab;
    [SerializeField] private float _introTime;
    [SerializeField] private Vector3 _replayPos;

    public GameObject LevelPrefab => _levelPrefab;
    public float IntroTime => _introTime;
    public Vector3 ReplayPos => _replayPos;
}