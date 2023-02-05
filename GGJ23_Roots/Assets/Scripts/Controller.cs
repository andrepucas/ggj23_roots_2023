using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("LEVELS")]
    [SerializeField] private Animator _anim;
    [SerializeField] private LevelDataSO _levelData;
    [SerializeField] private Transform _levelsFolder;
    
    [Header("ROOTS")]
    [SerializeField] private RootZigZag _rootZigZag;
    [SerializeField] private RootFlapper _rootFlapper;

    private PlayerInput _input;
    private InputAction _actionFlapper;

    private List<GameObject> _levels;
    private int _currentLevel;
    private bool _rootsMoving;

    private void Awake()
    {
        Cursor.visible = false;

        _input = GetComponent<PlayerInput>();
        _input.actions["ZigZag"].performed += _ => _rootZigZag.Move();
        //_actionZigZag = _input.actions["ZigZag"];
        //_actionFlapper = _input.actions["Flapper"];

        //_actionZigZag.performed += _ => _rootZigZag.Move();
        
        //_actionFlapper.started += _ => _rootFlapper.Move();
        //_actionFlapper.canceled -= _ => _rootFlapper.Move();

        GameObject levelRef;
        _levels = new List<GameObject>();

        foreach (Transform child in _levelsFolder)
            Destroy(child.gameObject);

        foreach (Level level in _levelData.Levels)
        {
            levelRef = Instantiate(level.Prefab);
            levelRef.SetActive(false);
            levelRef.transform.parent = _levelsFolder;
            _levels.Add(levelRef);
        }

        ChangeGameState(_levelData.StartGameState);
    }

    private void OnEnable()
    {
        AbstractRoot.Dead += GameOver;
        AbstractRoot.LevelEnd += () => StartCoroutine(NextLevel());
    }

    private void OnDisable()
    {
        AbstractRoot.Dead -= GameOver;
        AbstractRoot.LevelEnd -= () => StopCoroutine(NextLevel());
    }

    private void ChangeGameState(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.MENU:
                _input.SwitchCurrentActionMap("None");
                Debug.Log("Menu");

                _currentLevel = 0;
                _levels[0].SetActive(true);
                _anim.SetBool("Play", false);

                InputSystem.onAnyButtonPress.
                    CallOnce(ctrl => ChangeGameState(GameStates.LOAD_LEVEL));

                break;

            case GameStates.LOAD_LEVEL:
                _input.SwitchCurrentActionMap("None");
                Debug.Log("Load level");

                Vector3 position = transform.position;
                position.z = 0;

                _levels[_currentLevel].transform.position = position;
                _levels[_currentLevel].SetActive(true);

                _anim.SetInteger("Level", _currentLevel);
                _anim.SetBool("Play", true);

                StartCoroutine(WaitForIntro());

                break;

            case GameStates.GAMEPLAY:
                _input.SwitchCurrentActionMap("Gameplay");
                Debug.Log("Gameplay");

                if (!_rootsMoving) StartCoroutine(MoveRoots());

                break;
        }
    }

    private IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(_levelData.Levels[_currentLevel].IntroTime);
        ChangeGameState(GameStates.GAMEPLAY);
    }

    private void GameOver()
    {
        StopAllCoroutines();
        StartCoroutine(StopCamera());
        Debug.Log("Game Over");
    }

    private IEnumerator NextLevel()
    {
        _anim.SetBool("Play", false);
        _anim.SetTrigger("End");

        yield return new WaitForSeconds(2);

        _currentLevel++;

        if (_currentLevel < _levels.Count)
        {
            ChangeGameState(GameStates.LOAD_LEVEL);
            _levels[_currentLevel-1].SetActive(false);
        }

        else Debug.Log("No more levels.");
    }

    private IEnumerator MoveRoots()
    {
        _rootsMoving = true;
        
        while (true)
        {
            transform.position += _levelData.Levels[_currentLevel].Speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator StopCamera()
    {
        Vector3 startSpeed = _levelData.Levels[_currentLevel].Speed;
        float elapsedTime = 0;
        float currentSpeed = 0;

        while (startSpeed.magnitude >= 0)
        {
            currentSpeed = Mathf.Lerp(startSpeed.magnitude, 0, elapsedTime / 1);
            transform.position += startSpeed.normalized * currentSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
