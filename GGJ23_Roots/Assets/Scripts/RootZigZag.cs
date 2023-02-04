using UnityEngine;

public class RootZigZag : MonoBehaviour
{
    [Header("MOVEMENT")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    private bool _isControllable;
    private float _direction = 1;
    private Vector2 _velocity;

    private void FixedUpdate()
    {
        if (!_isControllable) return;

        _velocity.y = _direction * _speed;
        _rb.velocity += _velocity;
    }

    public void Move()
    {
        _isControllable = true;
        _direction =-_direction;

        Debug.Log(_direction);
    }
}