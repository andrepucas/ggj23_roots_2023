using UnityEngine.InputSystem;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("ROOTS")]
    [SerializeField] private RootZigZag _rootZigZag;

    private PlayerInput _input;
    private InputAction _actionZigZag;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();
        _actionZigZag = _input.actions["ZigZag"];

        _actionZigZag.performed += _ => _rootZigZag.Move();
    }

    private void Start()
    {
        _rootZigZag.enabled = true;
    }

    private void Update()
    {
        transform.position += Vector3.right * 0.008f;
    }
}
