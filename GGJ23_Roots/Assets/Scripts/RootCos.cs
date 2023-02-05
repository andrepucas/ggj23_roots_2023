using UnityEngine;

public class RootCos : AbstractRoot
{
    [Header("MOVEMENT")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    private bool _isControllable;
    private bool _stop;
    private float _direction = 0.5f;
    private Vector2 _velocity;


    public void Move()
    {
        _isControllable = true;
        _stop = true;
        
    }
    public void NotMove()
    {
        _isControllable = true;
        _stop = false;

    }

    private void FixedUpdate()
    {
        if (!_isControllable) return;
        
        if(!_stop)
            _direction = (Mathf.PingPong(Time.time/0.5f,2)-1);

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