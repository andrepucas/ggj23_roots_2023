using System;
using System.Collections;
using UnityEngine;

public class PlayableRoot : MonoBehaviour
{
    public static event Action OnDead;
    public static event Action OnLevelEnd;

    [Header("TRAITS")]
    [SerializeField] private GameObject _moveBehaviour;
    [SerializeField] private Color _mainColor, _particlesColor;
    
    [Header("COMPONENTS")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private ParticleSystem _particles;

    public IMoveBehaviour MoveBehaviour {get; set;}
    public Color MainColor {get; set;}
    public Color ParticlesColor {get; set;}

    private bool _isControllable;
    private ParticleSystem.MainModule _particlesModule;

    private void Awake() => _particlesModule = _particles.main;

    public void Reset()
    {
        _isControllable = false;
        MoveBehaviour = _moveBehaviour.GetComponent<IMoveBehaviour>();
        MainColor = _mainColor;
        ParticlesColor = _particlesColor;

        _rb.velocity = Vector2.zero;
        UpdateTraits();
    }

    public void UpdateTraits()
    {
        _sprite.color = MainColor;
        _trail.startColor = MainColor;
        _particlesModule.startColor = ParticlesColor;
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
            
            StartCoroutine(ReSpawning());
            OnDead?.Invoke();
        }

        else if (col.gameObject.tag == "EndLevel")
        {
            OnLevelEnd?.Invoke();
            _isControllable = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Root")
        {
            PlayableRoot other = col.gameObject.GetComponent<PlayableRoot>();

            if (other.GetInstanceID() < GetInstanceID()) return;
            
            Debug.Log("BOOP");

            IMoveBehaviour auxMove = other.MoveBehaviour;
            Color auxMainColor = other.MainColor;
            Color auxPartColor = other.ParticlesColor;

            // Change other.
            other.MoveBehaviour = MoveBehaviour;
            other.MainColor = MainColor;
            other.ParticlesColor = ParticlesColor;

            // Change self.
            MoveBehaviour = auxMove;
            MainColor = auxMainColor;
            ParticlesColor = auxPartColor;
            
            UpdateTraits();
            other.UpdateTraits();
        }
    }

    private IEnumerator ReSpawning()
    {
        Transform parent = transform.parent;
        transform.parent = null;
        _rb.velocity = Vector2.zero;

        yield return new WaitForSeconds(1.2f);
        transform.parent = parent;
    }
}
