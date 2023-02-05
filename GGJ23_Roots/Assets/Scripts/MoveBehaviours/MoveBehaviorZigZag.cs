using UnityEngine;

public class MoveBehaviorZigZag : MonoBehaviour, IMoveBehaviour
{
    [SerializeField] private float _speed = 5;
    
    private Vector2 _velocity;
    private int _direction = 1;

    public void HandleInput()
    {
        _direction = -_direction;
    }

    public Vector2 Move()
    {
        _velocity.y = _direction * _speed * Time.deltaTime;
        return _velocity;
    }
}
