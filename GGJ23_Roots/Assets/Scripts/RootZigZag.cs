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
            _isControllable = false;
            
            base.OnDead();
            StartCoroutine(ReSpawning());
        }

        else if (col.gameObject.tag == "EndLevel")
        {
            base.OnEndLevel();
            StartCoroutine(ResetPosition());
        }
    }

    private IEnumerator ReSpawning()
    {
        Transform parent = transform.parent;
        transform.parent = null;
        _rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(3);

        Debug.Log("Reset");
        transform.parent = parent;
        transform.localPosition = new Vector3(0, 0, 0);
    }

    private IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(2f);
        _isControllable = false;

        _rb.velocity = Vector2.zero;
        transform.localPosition = Vector3.zero;
    }
}