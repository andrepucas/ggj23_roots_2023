using UnityEngine;

public class RootFlapper : AbstractRoot
{
    [Header("MOVEMENT")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    private bool _isControllable;
    private float _direction = 0.5f;
    private Vector2 _velocity;

    public void Move()
    {
        _isControllable = true;
        _direction += -0.25f;
    }

    private void FixedUpdate()
    {
        if (!_isControllable) return;
        _velocity.y = _direction * _speed;
        _rb.velocity += _velocity;
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Obstacle")
        {
            base.OnDead();
            _isControllable = false;
            _rb.velocity = Vector2.zero;
            transform.parent = null;
            Destroy(this);
        }
    }
}