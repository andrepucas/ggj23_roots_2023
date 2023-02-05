using UnityEngine;

public class MoveBehaviorFlapper : MonoBehaviour, IMoveBehaviour
{
    [SerializeField] private float _speed = 5;
    
    private Vector2 _velocity;
    private float _direction = 0.5f;

    public void HandleInput()
    {
        _direction += 1f;

        if (_direction > 0.8f) _direction = 0.8f;
    }

    public Vector2 Move()
    {
        _direction -= 0.005f; 
        if (_direction < -0.8f) _direction = -0.8f;
        _velocity.y = _direction * _speed * Time.deltaTime;
        return _velocity;
    }
}
