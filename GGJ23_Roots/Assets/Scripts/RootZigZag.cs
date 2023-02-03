using UnityEngine;
using System.Collections;

public class RootZigZag : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    private bool _isControllable;
    private int _direction = 1;

    private void FixedUpdate()
    {
        if (!_isControllable) return;

        
        transform.position += Vector3.up * _direction * _speed;
    }

    public void Move()
    {
        _isControllable = true;
        _direction =-_direction;

        Debug.Log(_direction);
    }
}