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
    [SerializeField] private LevelRoot[] _roots;
    [SerializeField] private Vector3 _speed;
    [SerializeField] private float _introTime;
    [SerializeField] private Vector3 _replayPos;

    public GameObject Prefab => _prefab;
    public IReadOnlyList<LevelRoot> Roots => _roots;
    public Vector3 Speed => _speed;
    public float IntroTime => _introTime;
    public Vector3 ReplayPos => _replayPos;
}

[System.Serializable]
public struct LevelRoot
{
    [SerializeField] private int _id;
    [SerializeField] private Vector3 _spawn;

    public int ID => _id;
    public Vector3 SpawnPos => _spawn;
}