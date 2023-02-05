using UnityEngine;
using System.Collections;

public class RootZigZag : AbstractRoot
{
    [Header("MOVEMENT")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private float _speed;

    private bool _isControllable;
    private float _direction = 1;
    private Vector2 _velocity;

    public void Move()
    {
        _isControllable = true;
        _direction =-_direction;
    }

    private void Update()
    {
        if (!_isControllable) return;

        _velocity.y = _direction * _speed * Time.deltaTime;
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

        else if (col.gameObject.tag == "EndLevel")
        {
            base.OnEndLevel();
            StartCoroutine(ResetPosition());
        }
    }

    private IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(2f);
        _isControllable = false;

        _rb.velocity = Vector2.zero;
        transform.localPosition = Vector3.zero;
    }
}