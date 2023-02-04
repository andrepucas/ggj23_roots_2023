using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("ROOTS")]
    [SerializeField] private Transform _rootsParent;
    [SerializeField] private RootZigZag _rootZigZag;
    [SerializeField] private float _rootsStartPosition;
    [SerializeField] private float _rootsPosition;

    private PlayerInput _input;
    private InputAction _actionZigZag;

    private float _levelSpeed = 0.008f;
    private float _currentSpeed;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _actionZigZag = _input.actions["ZigZag"];

        _actionZigZag.performed += _ => _rootZigZag.Move();
    }

    private void OnEnable()
    {
        AbstractRoot.Dead += GameOver;
    }

    private void OnDisable()
    {
        AbstractRoot.Dead -= GameOver;
    }

    private void Start()
    {
        _rootZigZag.enabled = true;
        StartCoroutine(MoveRoots());
    }

    private void GameOver()
    {
        StopAllCoroutines();
        StartCoroutine(StopCamera());
    }

    private IEnumerator MoveRoots()
    {
        _currentSpeed = 0;
        Vector3 _rootsPos = _rootsParent.transform.position;
        float elapsedTime = 0;
        
        
        while (true)
        {
            if (_currentSpeed < _levelSpeed)
            {
                _currentSpeed = Mathf.Lerp(0, _levelSpeed, elapsedTime / 3);
                _rootsPos.x = Mathf.Lerp(_rootsStartPosition, _rootsPosition, elapsedTime / 3);
                _rootsPos.z = 0;
                _rootsParent.transform.localPosition = _rootsPos;
            }

            transform.position += Vector3.right * _currentSpeed;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator StopCamera()
    {
        float startSpeed = _currentSpeed;
        float elapsedTime = 0;

        while (_currentSpeed >= 0)
        {
            _currentSpeed = Mathf.Lerp(startSpeed, 0, elapsedTime / 1);
            transform.position += Vector3.right * _currentSpeed;
            _rootsParent.position += Vector3.right * (_levelSpeed - _currentSpeed);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
