using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Data/Level Data")]
public class LevelDataSO : ScriptableObject
{
    [SerializeField] private GameStates _startGameState;
    [SerializeField] private Level[] _levels;

    public GameStates StartGameState => _startGameState;
    public IReadOnlyList<Level> Levels => _levels;
}

[System.Serializable]
public struct Level
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Vector3 _speed;
    [SerializeField] private float _introTime;
    [SerializeField] private Vector3 _replayPos;

    public GameObject Prefab => _prefab;
    public Vector3 Speed => _speed;
    public float IntroTime => _introTime;
    public Vector3 ReplayPos => _replayPos;
}