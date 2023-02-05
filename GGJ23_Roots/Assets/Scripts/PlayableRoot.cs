using System;
using System.Collections;
using UnityEngine;

public class PlayableRoot : MonoBehaviour
{
    public static event Action OnDead;
    public static event Action OnLevelEnd;

    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private GameObject _moveBehaviour;

    public IMoveBehaviour MoveBehaviour {get; set;}

    private bool _isControllable;

    public void Start()
    {
        MoveBehaviour = _moveBehaviour.GetComponent<IMoveBehaviour>();
        _rb.velocity = Vector2.zero;
    }
    
    public void Interact()
    {
        _isControllable = true;
        MoveBehaviour.HandleInput();
    }
    
    private void Update()
    {
        if (_isControllable) _rb.velocity += MoveBehaviour.Move();
    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            _isControllable = false;
            
            OnDead?.Invoke();
            StartCoroutine(ReSpawning());
        }

        else if (col.gameObject.tag == "EndLevel")
        {
            OnLevelEnd?.Invoke();
            _isControllable = false;
        }
    }

    private IEnumerator ReSpawning()
    {
        Transform parent = transform.parent;
        transform.parent = null;
        _rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(2);

        Debug.Log("Respawn");
        transform.parent = parent;
    }
}
