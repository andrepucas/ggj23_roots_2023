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
    [SerializeField] private Transform _rootsParent;
    [SerializeField] private RootZigZag _rootZigZag;
    [SerializeField] private RootFlapper _rootFlapper;
    [SerializeField] private RootCos _rootCos;
    [SerializeField] private float _rootsStartPosition;
    [SerializeField] private float _rootsPosition;

    private PlayerInput _input;
    private InputAction _actionFlapper;

    private List<GameObject> _levels;
    private int _currentLevel;

    private void Awake()
    {
        Cursor.visible = false;

        _input = GetComponent<PlayerInput>();
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
    }

    private void OnDisable()
    {
        AbstractRoot.Dead -= GameOver;
    }

    // private void Start()
    // {
    //     //if (!_rootFlapper) FindObjectOfType<RootFlapper>();
    //     _rootZigZag.enabled = true;
    //     //_rootFlapper.enabled = true;
    //     StartCoroutine(MoveRoots());
    // }

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

                _levelData.Levels[_currentLevel].Prefab.SetActive(true);

                _anim.SetInteger("Level", _currentLevel);
                _anim.SetBool("Play", true);

                StartCoroutine(WaitForIntro());

                break;

            case GameStates.GAMEPLAY:
                _input.SwitchCurrentActionMap("Gameplay");
                Debug.Log("Gameplay");

                _input.actions["ZigZag"].performed += _ => _rootZigZag.Move();
                _input.actions["Flapper"].started += _ => StartCoroutine("FlapperGo");
                _input.actions["Flapper"].canceled += _ => StopCoroutine("FlapperGo");
                _input.actions["Cos"].started += _ => _rootCos.NotMove();
                _input.actions["Cos"].canceled -= _ => _rootCos.Move();
                StartCoroutine(MoveRoots());

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
        //StartCoroutine(StopCamera());
        Debug.Log("Game Over");
    }

    private IEnumerator MoveRoots()
    {
        while (true)
        {
            transform.position += _levelData.Levels[_currentLevel].Speed * Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator FlapperGo()
    {
        while (true)
        {
            _rootFlapper.Move();
            yield return null;
        }
    }
    private IEnumerator CosGo()
    {
        while (true)
        {
            _rootCos.Move();
            yield return null;
        }
    }

    // private IEnumerator StopCamera()
    // {
    //     Vector3 startSpeed = _levelData.Levels[_currentLevel].Speed;
    //     float elapsedTime = 0;

    //     while (startSpeed.magnitude >= 0)
    //     {
    //         _currentSpeed = Mathf.Lerp(startSpeed, 0, elapsedTime / 1);
    //         transform.position += Vector3.right * _currentSpeed;
    //         _rootsParent.position += Vector3.right * (_levelSpeed - _currentSpeed);

    //         elapsedTime += Time.deltaTime;
    //         yield return null;
    //     }
    // }
}
