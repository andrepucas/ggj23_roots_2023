using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private UIPanels _ui;

    [Header("LEVELS")]
    [SerializeField] private Animator _anim;
    [SerializeField] private LevelDataSO _levelData;
    [SerializeField] private Transform _levelsFolder;
    
    [Header("ROOTS")]
    [SerializeField] private RootZigZag _rootZigZag;
    [SerializeField] private RootFlapper _rootFlapper;
    [SerializeField] private RootCos _rootCos;
    [SerializeField] private float _rootsStartPosition;
    [SerializeField] private float _rootsPosition;


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

        StartCoroutine(RevealMenu());
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
                Debug.Log("Menu");

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

                _input.actions["Flapper"].started += _ => StartCoroutine("FlapperGo");
                _input.actions["Flapper"].canceled += _ => StopCoroutine("FlapperGo");
                _input.actions["Cos"].started += _ => _rootCos.NotMove();
                _input.actions["Cos"].canceled -= _ => _rootCos.Move();

                if (!_rootsMoving) StartCoroutine(MoveRoots());

                break;
        }
    }

    private IEnumerator RevealMenu()
    {
        _ui.RevealBlackPanel();
        _ui.HideWhitePanel();

        _input.SwitchCurrentActionMap("None");

        _currentLevel = 0;
        _levels[0].SetActive(true);
        _anim.SetBool("Play", false);

        _ui.HideBlackPanel(3);
        yield return new WaitForSeconds(1.5f);

        ChangeGameState(_levelData.StartGameState);
    }

    private IEnumerator WaitForIntro()
    {
        yield return new WaitForSeconds(_levelData.Levels[_currentLevel].IntroTime);
        ChangeGameState(GameStates.GAMEPLAY);
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
        StopAllCoroutines();
        StartCoroutine(StopCamera());
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

    private IEnumerator StopCamera()
    {
        _rootsMoving = false;
        
        Vector3 startSpeed = _levelData.Levels[_currentLevel].Speed;
        float elapsedTime = 0;
        float currentSpeed = 0;

        Debug.Log("Stopping");
        
        while (elapsedTime < 1)
        {
            currentSpeed = Mathf.Lerp(startSpeed.magnitude, 0, elapsedTime / 1);
            transform.position += startSpeed.normalized * currentSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("Fading Out");

        _ui.RevealWhitePanel(1);
        yield return new WaitForSeconds(1);

        Debug.Log("Moving Level");

        _levels[_currentLevel].transform.position = 
            _levelData.Levels[_currentLevel].ReplayPos + transform.position;

        StartCoroutine(MoveRoots());

        Debug.Log("Replay Anim");
        _anim.SetBool("Play", false);
        _anim.SetTrigger("Replay");

        yield return new WaitForSeconds(1f);
        _ui.HideWhitePanel(1);
        yield return new WaitForSeconds(1f);
        Debug.Log("Enable Input");
        _input.SwitchCurrentActionMap("Gameplay");
    }
}
