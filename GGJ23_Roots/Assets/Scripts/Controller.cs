using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] private UIPanels _ui;
    [SerializeField] private AudioSource _audio;

    [Header("LEVELS")]
    [SerializeField] private Animator _anim;
    [SerializeField] private LevelDataSO _levelData;
    [SerializeField] private Transform _levelsFolder;
    
    [Header("ROOTS")]
    [SerializeField] private List<PlayableRoot> _roots;

    private PlayerInput _input;
    private InputAction _actionFlapper;

    private List<GameObject> _levels;
    private int _currentLevel;
    private bool _rootsMoving;
    private bool _musicPlaying;
    private bool _gameRunning;
    private bool _switchingLevel;

    private void Awake()
    {
        Cursor.visible = false;
        _gameRunning = true;
        
        Vector3 resetPos = Vector3.zero;
        resetPos.z = -10;
        transform.position = resetPos;

        _input = GetComponent<PlayerInput>();
        _input.actions["0"].performed += _ => _roots[0]?.Interact();
        _input.actions["1"].performed += _ => _roots[1]?.Interact();

        foreach (Transform child in _levelsFolder)
            Destroy(child.gameObject);

        GameObject levelRef;
        _levels = new List<GameObject>();

        foreach (Level level in _levelData.Levels)
        {
            levelRef = Instantiate(level.Prefab);
            levelRef.SetActive(false);
            levelRef.transform.parent = _levelsFolder;
            _levels.Add(levelRef);
        }

        foreach (PlayableRoot root in _roots)
            root.gameObject.SetActive(false);

        StartCoroutine(RevealMenu());
    }

    private void OnEnable()
    {
        PlayableRoot.OnDead += GameOver;
        PlayableRoot.OnLevelEnd += () => {if (!_switchingLevel) StartCoroutine(NextLevel());};
    }

    private void OnDisable()
    {
        PlayableRoot.OnDead -= GameOver;
        PlayableRoot.OnLevelEnd -= () => StopCoroutine(NextLevel());
    }

    private void ChangeGameState(GameStates gameState)
    {
        switch (gameState)
        {
            case GameStates.MENU:
                _input.SwitchCurrentActionMap("None");
                Debug.Log("Menu");

                _musicPlaying = false;

                InputSystem.onAnyButtonPress.
                    CallOnce(ctrl => ChangeGameState(GameStates.LOAD_LEVEL));

                break;

            case GameStates.LOAD_LEVEL:
                _input.SwitchCurrentActionMap("None");
                Debug.Log("Load level");

                if (!_musicPlaying) StartCoroutine(MusicDelay());

                Vector3 position = transform.position;
                position.z = 0;

                int rootIndex;

                for (int i = 0; i < _levelData.Levels[_currentLevel].Roots.Count; i++)
                {
                    rootIndex = _levelData.Levels[_currentLevel].Roots[i].ID;

                    _roots[rootIndex].gameObject.SetActive(true);

                    _roots[rootIndex].Reset();

                    _roots[rootIndex].transform.localPosition = 
                        _levelData.Levels[_currentLevel].Roots[i].SpawnPos;
                }
                
                _levels[_currentLevel].transform.position = position;
                _levels[_currentLevel].SetActive(true);

                _anim.SetInteger("Level", _currentLevel);
                _anim.SetTrigger("Play");

                StartCoroutine(WaitForIntro());

                break;

            case GameStates.GAMEPLAY:
                _input.SwitchCurrentActionMap("Gameplay");
                Debug.Log("Gameplay");

                foreach (PlayableRoot root in _roots)
                    root.EnableInputHint();

                if (!_rootsMoving) StartCoroutine(MoveRoots());

                break;
        }
    }

    private IEnumerator MusicDelay()
    {
        _musicPlaying = true;
        yield return new WaitForSeconds(2.2f);
        _audio.Play();
    }

    private IEnumerator RevealMenu()
    {
        _ui.RevealBlackPanel();
        _ui.HideWhitePanel();

        _input.SwitchCurrentActionMap("None");

        _currentLevel = 0;
        _levels[0].SetActive(true);

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
        if (!_gameRunning) return;

        _gameRunning = false;

        Debug.Log("Game Over");
        StopAllCoroutines();
        StartCoroutine(StopCamera());

        for (int i = 0; i < _levelData.Levels[_currentLevel].Roots.Count; i++)
            _roots[_levelData.Levels[_currentLevel].Roots[i].ID].Stop();
    }

    private IEnumerator NextLevel()
    {
        _switchingLevel = true;

        Debug.Log(1);
        _input.SwitchCurrentActionMap("None");
        _anim.SetTrigger("End");

        yield return new WaitForSeconds(2);

        foreach (PlayableRoot root in _roots)
            root.gameObject.SetActive(false);

        if (_currentLevel < _levels.Count-1)
        {
            _currentLevel++;
            ChangeGameState(GameStates.LOAD_LEVEL);
            _levels[_currentLevel-1].SetActive(false);
        }

        else
        {
            _ui.RevealBlackPanel(2);
            _anim.SetTrigger("Last");
            yield return new WaitForSeconds(2);

            _levels[_currentLevel-1].SetActive(false);
            _currentLevel = 0;

            LastLevel();
        }

        _switchingLevel = false;
    }

    private void LastLevel()
    {
        StopAllCoroutines();
        _rootsMoving = false;

        Vector3 resetPos = Vector3.zero;
        resetPos.z = -10;
        transform.position = resetPos;

        for (int i = 0; i < _levelData.Levels[_currentLevel].Roots.Count; i++)
            _roots[_levelData.Levels[_currentLevel].Roots[i].ID].Stop();

        foreach (Transform level in _levelsFolder)
        {
            level.gameObject.SetActive(false);
            level.localPosition = Vector3.zero;
        }

        StartCoroutine(RevealMenu());
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
        _rootsMoving = false;
        _input.SwitchCurrentActionMap("None");
        
        Vector3 startSpeed = _levelData.Levels[_currentLevel].Speed;
        float elapsedTime = 0;
        float currentSpeed = 0;

        while (elapsedTime < 1)
        {
            currentSpeed = Mathf.Lerp(startSpeed.magnitude, 0, elapsedTime / 1);
            transform.position += startSpeed.normalized * currentSpeed * Time.deltaTime;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _ui.RevealWhitePanel(1);
        yield return new WaitForSeconds(1);

        _levels[_currentLevel].transform.position = 
            _levelData.Levels[_currentLevel].ReplayPos + transform.position;

        int rootIndex;
        
        for (int i = 0; i < _levelData.Levels[_currentLevel].Roots.Count; i++)
        {
            rootIndex = _levelData.Levels[_currentLevel].Roots[i].ID;

            _roots[rootIndex].Reset();
            
            _roots[rootIndex].transform.localPosition = 
                _levelData.Levels[_currentLevel].Roots[i].SpawnPos;
        }

        StartCoroutine(MoveRoots());

        _anim.SetTrigger("Replay");

        yield return new WaitForSeconds(2f);

        _ui.HideWhitePanel(1);
        yield return new WaitForSeconds(1f);

        _input.SwitchCurrentActionMap("Gameplay");

        foreach (PlayableRoot root in _roots)
            root.EnableInputHint();

        _gameRunning = true;
    }
}
